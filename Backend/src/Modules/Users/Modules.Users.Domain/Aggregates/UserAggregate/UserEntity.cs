using SharedKernel;
using SharedKernel.Abstractions;
using SharedKernel.Contracts;
using Modules.Users.Domain.Aggregates.UserAggregate.Events;
using Modules.Users.Domain.ValueObjects;
using StrictId;

namespace Modules.Users.Domain.Aggregates.UserAggregate;

public class UserEntity : AggregateRoot
{
    public Id<UserEntity> Id { get; private set; } = Id<UserEntity>.Empty;
    public Email Email { get; private set; }
    public string Password { get; private set; } = string.Empty;

    public static Result<UserEntity> Create(string email, string password)
    {
        var createEmailResult = Email.Create(email);

        if (createEmailResult.IsFailure)
        {
            return Result.Failure<UserEntity>(createEmailResult.DomainError);
        }

        var user = new UserEntity
        {
            Id = Id<UserEntity>.NewId(),
            Email = createEmailResult.Value,
            Password = password
        };

        user.Raise(new UserCreatedDomainEvent(user.Id));

        return user;
    }

    private UserEntity()
    {
    }
}