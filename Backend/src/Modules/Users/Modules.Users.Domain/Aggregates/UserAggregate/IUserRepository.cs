using SharedKernel.Contracts;
using SharedKernel.Contracts.Data;

namespace Modules.Users.Domain.Aggregates.UserAggregate;

public interface IUserRepository : IRepository
{
    void AddUser(UserEntity userEntity, CancellationToken cancellationToken);
}