using System.Collections.Concurrent;
using SharedKernel.Contracts.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class EventPublisher(IServiceProvider serviceProvider, ILogger<EventPublisher> logger) : IEventPublisher
{
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public async Task Publish<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken)
        where TEvent : IEvent
    {
        var eventType = @event.GetType();
        logger.LogDebug("Publishing event {EventType}", eventType.Name);

        try
        {
            using var scope = serviceProvider.CreateScope();

            var handlerType = HandlerTypeDictionary.GetOrAdd(
                eventType,
                et => typeof(IEventHandler<>).MakeGenericType(et));

            var handlers = scope.ServiceProvider.GetServices(handlerType).ToArray();
            if (handlers.Length == 0)
            {
                logger.LogDebug("No handlers registered for event {EventType}", eventType.Name);
                return;
            }

            logger.LogDebug("Found {HandlerCount} handlers for event {EventType}", handlers.Length, eventType.Name);

            var handlerTasks = handlers
                .Select(handler =>
                {
                    var wrapper = HandlerWrapper.Create(handler!, eventType, logger);
                    return wrapper.Handle(@event, cancellationToken);
                })
                .ToList();

            await Task.WhenAll(handlerTasks);

            var exceptions = handlerTasks
                .Where(t => t.IsFaulted)
                .SelectMany(t => t.Exception?.InnerExceptions ?? Enumerable.Empty<Exception>())
                .ToList();

            if (exceptions.Count > 0)
            {
                logger.LogError("One or more handlers threw exceptions while processing event {EventType}", eventType.Name);
                throw new AggregateException($"One or more handlers threw exceptions while processing event {eventType.Name}", exceptions);
            }

            logger.LogDebug("Successfully published event {EventType}", eventType.Name);
        }
        catch (AggregateException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error publishing event {EventType}", eventType.Name);
            throw;
        }
    }

    private abstract class HandlerWrapper
    {
        public abstract Task Handle(IEvent @event, CancellationToken cancellationToken);

        public static HandlerWrapper Create(object handler, Type eventType, ILogger logger)
        {
            var wrapperType = WrapperTypeDictionary.GetOrAdd(
                eventType,
                et => typeof(HandlerWrapper<>).MakeGenericType(et));

            return (HandlerWrapper)Activator.CreateInstance(wrapperType, handler, logger)!;
        }
    }

    private sealed class HandlerWrapper<TEvent>(object handler, ILogger logger) : HandlerWrapper
        where TEvent : IEvent
    {
        private readonly IEventHandler<TEvent> _handler = (IEventHandler<TEvent>)handler;

        public override async Task Handle(IEvent @event, CancellationToken cancellationToken)
        {
            try
            {
                await _handler.HandleAsync((TEvent)@event, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error handling event {EventType} with handler {HandlerType}",
                    typeof(TEvent).Name, _handler.GetType().Name);
                throw;
            }
        }
    }
}