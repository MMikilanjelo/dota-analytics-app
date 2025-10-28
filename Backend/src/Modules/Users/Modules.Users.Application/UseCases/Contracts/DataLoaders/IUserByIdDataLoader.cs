using Common.Contracts;
using Modules.Users.Domain.Aggregates.UserAggregate;
using StrictId;

namespace Modules.Users.Application.UseCases.Contracts.DataLoaders;

public interface IUserByIdDataLoader : IDataLoader<Id<UserEntity>, UserEntity>;