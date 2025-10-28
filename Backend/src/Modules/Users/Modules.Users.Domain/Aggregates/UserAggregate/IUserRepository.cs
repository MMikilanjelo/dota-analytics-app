using Common.Contracts;

namespace Modules.Users.Domain.Aggregates.UserAggregate;

public interface IUserRepository : IRepository
{
    void AddUser(UserEntity userEntity, CancellationToken cancellationToken);
}