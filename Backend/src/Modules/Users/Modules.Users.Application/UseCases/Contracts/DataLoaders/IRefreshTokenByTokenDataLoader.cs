using Common.Contracts;
using Modules.Users.Domain.Aggregates.RefreshTokenAggregate;

namespace Modules.Users.Application.UseCases.Contracts.DataLoaders;

public interface IRefreshTokenByTokenDataLoader : IDataLoader<string, RefreshTokenEntity>;