using GreenDonut;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Domain.Aggregates.UserAggregate;
using StrictId;

namespace Modules.Users.Infrastructure.Data.DataLoaders;

internal static class UsersDataLoader
{
    [DataLoader]
    internal static async Task<IReadOnlyDictionary<string, UserEntity>> UserByEmailDataLoader(
        IReadOnlyList<string> emails,
        UsersDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var users = await dbContext.Users
            .Where(u => emails.Contains(u.Email.Value))
            .ToListAsync(cancellationToken);

        return users.ToDictionary(u => u.Email.Value, u => u);
    }

    [DataLoader]
    internal static async Task<IReadOnlyDictionary<Id<UserEntity>, UserEntity>> UserByIdDataLoader(
        IReadOnlyList<Id<UserEntity>> ids,
        UsersDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var users = await dbContext.Users
            .Where(u => ids.Contains(u.Id))
            .ToListAsync(cancellationToken);

        return users.ToDictionary(u => u.Id, u => u);
    }
}

internal partial class UserByEmailDataLoader : Application.UseCases.Contracts.DataLoaders.IUserByEmailDataLoader;

internal partial class UserByIdDataLoader : Application.UseCases.Contracts.DataLoaders.IUserByIdDataLoader;