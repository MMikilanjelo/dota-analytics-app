using Modules.Users.Domain;
using Modules.Users.Domain.Aggregates.RefreshTokenAggregate;
using SharedKernel.Contracts.Data;

namespace Modules.Users.Infrastructure.Data.Repositories;

public class RefreshTokenRepository(UsersDbContext dbContext, IUsersUnitOfWork unitOfWork) : IRefreshTokenRepository
{
    public IUnitOfWork UnitOfWork => unitOfWork;

    public void AddRefreshToken(RefreshTokenEntity refreshTokenEntity, CancellationToken cancellationToken)
    {
        dbContext.RefreshTokens.Add(refreshTokenEntity);
    }

    public void UpdateRefreshToken(RefreshTokenEntity refreshTokenEntity, CancellationToken cancellationToken)
    {
        dbContext.RefreshTokens.Update(refreshTokenEntity);
    }
}