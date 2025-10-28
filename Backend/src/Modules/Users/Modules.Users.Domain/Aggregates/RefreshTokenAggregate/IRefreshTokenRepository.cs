using Common.Contracts;

namespace Modules.Users.Domain.Aggregates.RefreshTokenAggregate;

public interface IRefreshTokenRepository : IRepository
{
    void AddRefreshToken(RefreshTokenEntity refreshTokenEntity, CancellationToken cancellationToken);
    void UpdateRefreshToken(RefreshTokenEntity refreshTokenEntity, CancellationToken cancellationToken);
}