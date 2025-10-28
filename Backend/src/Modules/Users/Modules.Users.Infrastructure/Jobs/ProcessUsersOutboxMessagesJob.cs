using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Users.Infrastructure.Data;
using Newtonsoft.Json;
using Quartz;
using SharedKernel.Contracts.Messaging;

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

        await using var transaction = await usersDbContext.Database.BeginTransactionAsync(context.CancellationToken);

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

                var @event = JsonConvert.DeserializeObject<IEvent>(
                    message.Payload,
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
                );

                if (@event is null)
                {
                    message.Error = "Deserialization failed";
                    message.ProcessedOnUtc = DateTime.UtcNow;
                    continue;
                }

                await eventPublisher.Publish(@event, context.CancellationToken);

                message.ProcessedOnUtc = DateTime.UtcNow;

                logger.LogInformation("Successfully processed outbox message {MessageId} ({MessageType})", message.Id, message.Type);
            }
            catch (Exception ex)
            {
                message.Error = ex.Message;
                message.ProcessedOnUtc = DateTime.UtcNow;
                logger.LogError(ex, "Error processing outbox message {MessageId} ({MessageType})", message.Id, message.Type);
            }
        }

        await usersDbContext.SaveChangesAsync(context.CancellationToken);

        await transaction.CommitAsync(context.CancellationToken);

        logger.LogInformation("Completed {Job} at {Time}", nameof(ProcessUsersOutboxMessagesJob), DateTime.UtcNow);
    }
}