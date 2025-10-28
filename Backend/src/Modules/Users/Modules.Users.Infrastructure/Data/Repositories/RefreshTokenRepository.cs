using System.Threading;
using System.Threading.Tasks;
using Common.Contracts;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Domain;
using Modules.Users.Domain.Aggregates.RefreshTokenAggregate;

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