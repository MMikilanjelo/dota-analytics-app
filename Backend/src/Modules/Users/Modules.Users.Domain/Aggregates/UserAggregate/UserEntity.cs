using Common.Contracts;
using Modules.Users.Domain.Aggregates.UserAggregate.Events;
using Modules.Users.Domain.ValueObjects;
using StrictId;

namespace Modules.Users.Domain.Aggregates.UserAggregate;

public class UserEntity : AggreggateRoot
{
    public Id<UserEntity> Id { get; private set; } = Id<UserEntity>.Empty;
    public Email Email { get; private set; }
    public string Password { get; private set; } = string.Empty;

    public static UserEntity Create(string email, string password)
    {
        var user = new UserEntity
        {
            Id = Id<UserEntity>.NewId(),
            Email = Email.Create(email),
            Password = password
        };

        user.Raise(new UserCreatedDomainEvent(user.Id));

        return user;
    }

    private UserEntity()
    {
    }
}