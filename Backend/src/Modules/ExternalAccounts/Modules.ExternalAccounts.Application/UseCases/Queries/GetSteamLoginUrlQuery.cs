using Common;
using Common.Contracts.Messaging;
using FluentValidation;
using Modules.ExternalAccounts.Application.Common.Contracts.Clients;
using Modules.ExternalAccounts.Application.Common.Contracts.Services;

namespace Modules.ExternalAccounts.Application.UseCases.Queries;

public sealed record GetSteamLoginUrlQuery(
    string Scheme,
    string? Host,
    string? Path,
    string? RedirectUri
) : IQuery<string>;

internal sealed class GetSteamLoginUrlQueryHandler(
    ISteamClient steamClient,
    IUserContextService userContextService
) : IQueryHandler<GetSteamLoginUrlQuery, string>
{
    public async Task<Result<string>> HandleAsync(GetSteamLoginUrlQuery query, CancellationToken ct)
    {
        var url = await steamClient.BuildRedirectToLoginUrl(
            query.Scheme,
            query.Host ?? "",
            query.Path ?? "",
            query.RedirectUri ?? "",
            userContextService.UserId.ToString()
        );

        return Result.Success(url);
    }
}

internal sealed class GetSteamLoginUrlQueryValidator : AbstractValidator<GetSteamLoginUrlQuery>
{
    public GetSteamLoginUrlQueryValidator()
    {
        RuleFor(x => x.RedirectUri)
            .NotEmpty().WithMessage("redirect_uri is required.")
            .WithMessage("Invalid redirect_uri");
    }
}