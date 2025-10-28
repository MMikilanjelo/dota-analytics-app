using Common;
using Common.Contracts.Messaging;
using Common.Time;
using Modules.Users.Application.UseCases.Contracts.DataLoaders;
using Modules.Users.Application.UseCases.Contracts.Services;
using Modules.Users.Domain.Aggregates.RefreshTokenAggregate;

namespace Modules.Users.Application.UseCases.Commands;

public sealed record RefreshAccessTokenCommand(string RefreshToken) : ICommand<RefreshAccessTokenPayload>;

public sealed record RefreshAccessTokenPayload(string AccessToken, string RefreshToken);

public sealed record RefreshTokenExpiredError() : DomainError("RefreshTokens.Lifetime", "Refresh tokens expired");

public sealed record RefreshTokenNotFoundError() : DomainError("RefreshTokens.NotFound", "Refresh token not found");

public sealed class RefreshAccessTokenCommandHandler(
    IRefreshTokenRepository tokenRepository,
    IRefreshTokenByTokenDataLoader dataLoader,
    IIdentityService tokenProvider,
    IClock clock
) : ICommandHandler<RefreshAccessTokenCommand, RefreshAccessTokenPayload>
{
    public async Task<Result<RefreshAccessTokenPayload>> HandleAsync(RefreshAccessTokenCommand command, CancellationToken cancellationToken)
    {
        var refreshToken = await dataLoader.LoadAsync(command.RefreshToken, cancellationToken);

        if (refreshToken == null)
        {
            return Result.Failure<RefreshAccessTokenPayload>(new RefreshTokenNotFoundError());
        }

        if (refreshToken.IsExpired(clock.UtcNow))
        {
            return Result.Failure<RefreshAccessTokenPayload>(new RefreshTokenExpiredError());
        }

        var newRefreshToken = tokenProvider.GenerateRefreshToken();

        refreshToken.Refresh(clock.UtcNow, newRefreshToken);

        tokenRepository.UpdateRefreshToken(refreshToken, cancellationToken);

        var newAccessToken = tokenProvider.GenerateAccessToken(refreshToken.User);

        await tokenRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new RefreshAccessTokenPayload(newAccessToken, refreshToken.Token));
    }
}