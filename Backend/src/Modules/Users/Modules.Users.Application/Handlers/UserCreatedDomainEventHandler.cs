using SharedKernel.Contracts.Messaging;
using Modules.Users.Domain.Aggregates.UserAggregate.Events;
using Modules.Users.IntegrationEvents;

namespace Modules.Users.Application.Handlers;

public class UserCreatedDomainEventHandler(IEventPublisher eventPublisher) : IEventHandler<UserCreatedDomainEvent>
{
    public async Task HandleAsync(UserCreatedDomainEvent evt, CancellationToken cancellationToken)
    {
        var integrationEvent = new UserCreatedIntegrationEvent(evt.UserId.ToGuid());

        await eventPublisher.Publish(integrationEvent, cancellationToken);
    }
}