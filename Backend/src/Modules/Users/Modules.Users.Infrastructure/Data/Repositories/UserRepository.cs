using Modules.Users.Domain;
using Modules.Users.Domain.Aggregates.UserAggregate;
using SharedKernel.Contracts.Data;

namespace Modules.Users.Infrastructure.Data.Repositories;

public class UserRepository(UsersDbContext dbContext, IUsersUnitOfWork unitOfWork) : IUserRepository
{
    public IUnitOfWork UnitOfWork => unitOfWork;

    public void AddUser(UserEntity userEntity, CancellationToken cancellationToken)
    {
        dbContext.Users.Add(userEntity);
    }
}