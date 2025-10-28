namespace Common.Contracts.Events;

public interface IEventPublisher
{
    Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent;
}
