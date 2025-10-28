using SharedKernel;
using SharedKernel.Abstractions;
using SharedKernel.Contracts;
using Modules.Users.Domain.Aggregates.UserAggregate;
using SharedKernel.Errors;
using StrictId;

namespace Modules.Users.Domain.Aggregates.RefreshTokenAggregate;

public class RefreshTokenEntity : AggregateRoot
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

    public static Result<RefreshTokenEntity> Create(
        UserEntity userEntity,
        DateTime now,
        string token
    )
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Failure<RefreshTokenEntity>(new ValidationError([new NullValueError()]));
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