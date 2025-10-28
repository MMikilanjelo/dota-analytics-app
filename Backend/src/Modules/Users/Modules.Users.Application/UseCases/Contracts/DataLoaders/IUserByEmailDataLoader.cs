using SharedKernel.Contracts;
using SharedKernel.Contracts.Data;
using Modules.Users.Domain.Aggregates.UserAggregate;

namespace Modules.Users.Application.UseCases.Contracts.DataLoaders;

public interface IUserByEmailDataLoader : IDataLoader<string, UserEntity>;