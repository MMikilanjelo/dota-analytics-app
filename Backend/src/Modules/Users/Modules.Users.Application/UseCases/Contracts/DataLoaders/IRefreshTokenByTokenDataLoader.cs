using SharedKernel.Contracts;
using SharedKernel.Contracts.Data;
using Modules.Users.Domain.Aggregates.RefreshTokenAggregate;

namespace Modules.Users.Application.UseCases.Contracts.DataLoaders;

public interface IRefreshTokenByTokenDataLoader : IDataLoader<string, RefreshTokenEntity>;