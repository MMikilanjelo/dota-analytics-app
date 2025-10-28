using Common.Contracts;
using StrictId;

namespace Modules.Users.Domain.Aggregates.UserAggregate.Events;

public sealed record UserCreatedDomainEvent(Id<UserEntity> UserId) : IDomainEvent;