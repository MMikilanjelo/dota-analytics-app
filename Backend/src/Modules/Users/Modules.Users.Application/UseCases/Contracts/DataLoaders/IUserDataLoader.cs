using System.Linq.Expressions;

namespace Modules.Users.Application.UseCases.Contracts.DataLoaders;

public interface IUserDataLoader<in TKey, TValue> where TKey : notnull
{
    Task<TValue?> LoadAsync(
        TKey key,
        Expression<Func<TValue, TValue?>>? selector = default,
        CancellationToken cancellationToken = default
    );
}