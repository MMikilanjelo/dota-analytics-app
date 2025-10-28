using Common.Contracts;
using Modules.Users.Domain.Aggregates.UserAggregate;

namespace Modules.Users.Application.UseCases.Contracts.DataLoaders;

public interface IUserByEmailDataLoader : IDataLoader<string, UserEntity>;