using Common.Contracts;
using Modules.Users.Domain.Aggregates.UserAggregate;
using StrictId;

namespace Modules.Users.Domain.Aggregates.RefreshTokenAggregate;

public class RefreshTokenEntity : AggreggateRoot
{
    public required Id<RefreshTokenEntity> Id { get; init; }
    public required Id<UserEntity> UserId { get; init; }
    public DateTime ExpiresOnUtc { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public UserEntity User { get; init; }

    public bool IsExpired(DateTime now) =>
        ExpiresOnUtc <= now;

    public void Refresh(DateTime now, string token)
    {
        ExpiresOnUtc = now.AddDays(7);
        Token = token;
    }

    public static RefreshTokenEntity Create(
        UserEntity userEntity,
        DateTime now,
        string token
    )
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token cannot be empty.");
        }

        return new RefreshTokenEntity
        {
            Id = Id<RefreshTokenEntity>.NewId(),
            UserId = userEntity.Id,
            ExpiresOnUtc = now.AddDays(7),
            Token = token
        };
    }

    private RefreshTokenEntity()
    {
    }
}