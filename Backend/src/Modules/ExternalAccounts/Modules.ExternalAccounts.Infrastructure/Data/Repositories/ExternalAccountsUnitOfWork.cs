using Common.Contracts;
using Common.Outbox;
using Modules.ExternalAccounts.Domain;
using Newtonsoft.Json;

namespace Modules.ExternalAccounts.Infrastructure.Data.Repositories;

public class ExternalAccountsUnitOfWork(ExternalAccountsDbContext dbContext) : IExternalAccountsUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConvertDomainEventsToOutboxMessages();

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private void ConvertDomainEventsToOutboxMessages()
    {
        var messages = dbContext.ChangeTracker
            .Entries<AggreggateRoot>()
            .Select(e => e.Entity)
            .SelectMany(e =>
                {
                    var events = e.DomainEvents;

                    e.ClearDomainEvents();

                    return events;
                }
            )
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccuredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Payload = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }
                )
            })
            .ToList();

        dbContext.OutboxMessages.AddRange(messages);
    }
}