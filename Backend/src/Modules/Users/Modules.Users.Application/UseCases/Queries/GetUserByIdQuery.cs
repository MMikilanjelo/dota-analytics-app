using SharedKernel;
using SharedKernel.Contracts.Messaging;
using Modules.Users.Application.UseCases.Contracts.DataLoaders;
using Modules.Users.Domain.Aggregates.UserAggregate;
using SharedKernel.Errors;
using StrictId;
using User = Modules.Users.Application.Common.Models.User;

namespace Modules.Users.Application.UseCases.Queries;

public sealed record GetUserByIdQuery(Id<UserEntity> Id) : IQuery<User>;

internal sealed class GetUserByIdQueryHandler(IUserByIdDataLoader userByIdDataLoader) : IQueryHandler<GetUserByIdQuery, User>
{
    public async Task<Result<User>> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await userByIdDataLoader.LoadAsync(query.Id, cancellationToken);

        if (user == null)
        {
            return Result.Failure<User>(new ProblemError("Users.NotFound", "User not found"));
        }

        return Result.Success(new User
        {
            Id = user.Id.ToString(),
            Email = user.Email.Value,
        });
    }
}