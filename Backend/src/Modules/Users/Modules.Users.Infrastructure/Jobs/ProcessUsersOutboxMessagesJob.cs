using Common.Contracts;
using Common.Contracts.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Users.Infrastructure.Data;
using Newtonsoft.Json;
using Quartz;

namespace Modules.Users.Infrastructure.Jobs;

public class ProcessUsersOutboxMessagesJob(
    UsersDbContext usersDbContext,
    IEventPublisher eventPublisher,
    ILogger<ProcessUsersOutboxMessagesJob> logger
) : IJob
{
    public static readonly JobKey Key = JobKey.Create(nameof(ProcessUsersOutboxMessagesJob));

    private const int BatchSize = 50;

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Starting {Job} at {Time}", nameof(ProcessUsersOutboxMessagesJob), DateTime.UtcNow);

        var outboxMessages = await usersDbContext
            .OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccuredOnUtc)
            .Take(BatchSize)
            .ToListAsync(context.CancellationToken);

        logger.LogInformation("Fetched {Count} outbox messages to process", outboxMessages.Count);

        foreach (var message in outboxMessages)
        {
            try
            {
                logger.LogDebug("Processing outbox message {MessageId} of type {MessageType}", message.Id, message.Type);

                var eventType = Type.GetType(message.Type);

                if (eventType is null)
                {
                    logger.LogWarning("Unknown event type {EventType}", message.Type);
                    message.Error = "Unknown event type";
                    message.ProcessedOnUtc = DateTime.UtcNow;
                    continue;
                }

                var domainEvent = (IDomainEvent?)JsonConvert.DeserializeObject(
                    message.Payload,
                    eventType,
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                if (domainEvent is null)
                {
                    logger.LogWarning("Failed to deserialize message {MessageId} of type {MessageType}", message.Id, message.Type);

                    message.Error = "Deserialization failed";

                    message.ProcessedOnUtc = DateTime.UtcNow;

                    continue;
                }

                await eventPublisher.Publish(domainEvent, context.CancellationToken);

                message.ProcessedOnUtc = DateTime.UtcNow;

                logger.LogInformation("Successfully processed outbox message {MessageId} ({MessageType})", message.Id, message.Type);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing outbox message {MessageId} ({MessageType})", message.Id, message.Type);

                message.Error = ex.Message;

                message.ProcessedOnUtc = DateTime.UtcNow;
            }
        }

        await usersDbContext.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation("Completed {Job} at {Time}", nameof(ProcessUsersOutboxMessagesJob), DateTime.UtcNow);
    }
}