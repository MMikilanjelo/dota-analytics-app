using Microsoft.EntityFrameworkCore;
using Modules.Users.Domain.Aggregates.RefreshTokenAggregate;

namespace Modules.Users.Infrastructure.Data.DataLoaders;

internal static class RefreshTokensDataLoader
{
    [DataLoader]
    internal static async Task<IReadOnlyDictionary<string, RefreshTokenEntity>> RefreshTokenByTokenDataLoader(
        IReadOnlyList<string> tokens,
        UsersDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var entities = await dbContext.RefreshTokens
            .Where(t => tokens.Contains(t.Token))
            .Include(t => t.User)
            .ToListAsync(cancellationToken);

        return entities.ToDictionary(t => t.Token, t => t);
    }
}

internal partial class RefreshTokenByTokenDataLoader : Application.UseCases.Contracts.DataLoaders.IRefreshTokenByTokenDataLoader;