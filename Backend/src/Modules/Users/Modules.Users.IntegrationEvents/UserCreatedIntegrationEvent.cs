using Common.Contracts.Events;

namespace Modules.Users.IntegrationEvents;

public sealed record UserCreatedIntegrationEvent(Guid UserId) : IntegrationEvent;