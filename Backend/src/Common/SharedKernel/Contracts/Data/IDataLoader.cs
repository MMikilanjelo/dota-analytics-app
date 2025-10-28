namespace SharedKernel.Contracts.Data;

public interface IDataLoader<in TKey, TValue> where TKey : notnull
{
    Task<TValue?> LoadAsync(
        TKey key,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<TValue?>> LoadAsync(
        IReadOnlyCollection<TKey> keys,
        CancellationToken cancellationToken = default);
}