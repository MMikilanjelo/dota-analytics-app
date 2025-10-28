using System.Linq.Expressions;
using SharedKernel;
using SharedKernel.Contracts;
using SharedKernel.Contracts.Messaging;
using SharedKernel.Contracts.Time;
using FluentValidation;
using Modules.Users.Application.Common.Models;
using Modules.Users.Application.UseCases.Contracts.DataLoaders;
using Modules.Users.Application.UseCases.Contracts.Services;
using Modules.Users.Domain.Aggregates.RefreshTokenAggregate;
using Modules.Users.Domain.Aggregates.UserAggregate;
using SharedKernel.Errors;
using StrictId;

namespace Modules.Users.Application.UseCases.Commands;

public sealed record LoginUserCommand(
    string Email,
    string Password
) : ICommand<LoginUserPayload>;

public sealed record InvalidCredentialsError() : DomainError("Users.InvalidCredentials", "Invalid email or password.");

public sealed record UserNotFoundError : DomainError
{
    public UserNotFoundError(string email) : base("Users.NotFound", $"User with email '{email}' does not exist.")
    {
    }
}

public sealed record LoginUserPayload(User User, string AccessToken, string RefreshToken);

internal class CommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUserByEmailDataLoader userByEmailDataLoader,
    IPasswordHasherService passwordHasherService,
    IIdentityService identityService,
    IClock clock
) : ICommandHandler<LoginUserCommand, LoginUserPayload>
{
    public async Task<Result<LoginUserPayload>> HandleAsync(LoginUserCommand loginUserCommand, CancellationToken cancellationToken)
    {
        var user = await userByEmailDataLoader.LoadAsync(
            loginUserCommand.Email,
            cancellationToken
        );

        if (user == null)
        {
            return Result.Failure<LoginUserPayload>(new UserNotFoundError(loginUserCommand.Email));
        }

        if (!passwordHasherService.Verify(loginUserCommand.Password, user.Password))
        {
            return Result.Failure<LoginUserPayload>(new InvalidCredentialsError());
        }

        var accessToken = identityService.GenerateAccessToken(user);

        var createRefreshTokenResult = RefreshTokenEntity.Create(
            user,
            clock.UtcNow,
            identityService.GenerateRefreshToken()
        );

        if (createRefreshTokenResult.IsFailure)
        {
            return Result.Failure<LoginUserPayload>(createRefreshTokenResult.DomainError);
        }

        refreshTokenRepository.AddRefreshToken(createRefreshTokenResult.Value, cancellationToken);

        var response = new LoginUserPayload(
            new User
            {
                Id = user.Id.ToString(),
                Email = user.Email.Value,
            },
            accessToken,
            createRefreshTokenResult.Value.Token
        );

        await refreshTokenRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(response);
    }
}

internal sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}