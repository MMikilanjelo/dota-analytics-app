using Api.Types.Inputs;
using Common;
using Common.Contracts.Messaging;
using HotChocolate.Execution.Processing;
using Modules.Users.Application.Common.Models;
using Modules.Users.Application.UseCases.Commands;
using Modules.Users.Application.UseCases.Queries;
using Modules.Users.Domain.Aggregates.UserAggregate;
using StrictId;

namespace Api.Types;

public static class UsersOperations
{
    [Query]
    [NodeResolver]
    public static async Task<User?> GetUserById(
        string id,
        [Service] IQueryHandler<GetUserByIdQuery, User> handler,
        CancellationToken cancellationToken
    )
    {
        var query = new GetUserByIdQuery(new Id<UserEntity>(id));

        var user = await handler.HandleAsync(query, cancellationToken);

        return user.Value;
    }

    [Mutation]
    [Error<ValidationError>]
    [Error<NotUniqueEmailError>]
    public static async Task<FieldResult<User>> CreateUser(
        CreateUserInput input,
        [Service] ICommandHandler<CreateUserCommand, User> handler,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateUserCommand(input.Email, input.Password);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsFailure
            ? new FieldResult<User>(result.DomainError)
            : result.Value;
    }

    [Mutation]
    [Error<UserNotFoundError>]
    [Error<InvalidCredentialsError>]
    [Error<ValidationError>]
    public static async Task<FieldResult<LoginUserPayload>> LoginUser(
        LoginUserInput input,
        [Service] ICommandHandler<LoginUserCommand, LoginUserPayload> handler,
        CancellationToken cancellationToken
    )
    {
        var command = new LoginUserCommand(input.Email, input.Password);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsFailure
            ? new FieldResult<LoginUserPayload>(result.DomainError)
            : result.Value;
    }

    [Mutation]
    [Error<RefreshTokenExpiredError>]
    [Error<RefreshTokenNotFoundError>]
    public static async Task<FieldResult<RefreshAccessTokenPayload>> RefreshAccessToken(
        RefreshAccessTokenInput input,
        [Service] ICommandHandler<RefreshAccessTokenCommand, RefreshAccessTokenPayload> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new RefreshAccessTokenCommand(input.RefreshToken), cancellationToken);

        return result.IsFailure
            ? new FieldResult<RefreshAccessTokenPayload>(result.DomainError)
            : result.Value;
    }
}