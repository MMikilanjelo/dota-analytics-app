using SharedKernel.Contracts.Messaging;

namespace Modules.Users.IntegrationEvents;

public sealed record UserCreatedIntegrationEvent(Guid UserId) : IntegrationEvent;