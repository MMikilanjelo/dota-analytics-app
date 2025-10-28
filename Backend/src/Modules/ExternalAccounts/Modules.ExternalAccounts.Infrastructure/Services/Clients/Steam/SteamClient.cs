using System.Text.Json;
using System.Web;
using Common;
using Modules.ExternalAccounts.Application.Common.Contracts.Clients;
using Modules.ExternalAccounts.Application.Common.Contracts.Services;
using Modules.ExternalAccounts.Infrastructure.Services.Clients.Steam.Models;

namespace Modules.ExternalAccounts.Infrastructure.Services.Clients.Steam;

internal sealed class SteamClient(HttpClient httpClient, ILinkTokenService linkTokenService) : ISteamClient
{
    public async Task<string> BuildRedirectToLoginUrl(
        string scheme,
        string host,
        string pathBase,
        string redirectUri,
        string userId
    )
    {
        var linkToken = await linkTokenService.CreateTokenAsync(userId);

        var state = new StatePayload(redirectUri, linkToken);
        var stateJson = JsonSerializer.Serialize(state);

        var callbackUrl = $"{scheme}://{host}{pathBase}/api/external/steam/callback" +
                          $"?{SteamOpenId.StateParam}={Uri.EscapeDataString(stateJson)}";

        var query = HttpUtility.ParseQueryString(string.Empty);
        query[SteamOpenId.NsParam] = SteamOpenId.Ns;
        query[SteamOpenId.ModeParam] = SteamOpenId.ModeCheckId;
        query[SteamOpenId.ReturnToParam] = callbackUrl;
        query[SteamOpenId.RealmParam] = $"{scheme}://{host}/";
        query[SteamOpenId.IdentityParam] = SteamOpenId.IdentifierSelect;
        query[SteamOpenId.ClaimedIdParam] = SteamOpenId.IdentifierSelect;

        return $"{SteamOpenId.Url}?{query}";
    }

    public async Task<Result<SteamCallbackPayload>> ValidateCallbackAsync(
        IDictionary<string, string> queryParams,
        CancellationToken cancellationToken
    )
    {
        // 1. Verify with Steam
        var form = new Dictionary<string, string>(queryParams)
        {
            [SteamOpenId.ModeParam] = SteamOpenId.ModeCheckAuth
        };

        using var content = new FormUrlEncodedContent(form);

        var response = await httpClient.PostAsync("", content, cancellationToken);

        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!body.Contains(SteamOpenId.ValidResponse, StringComparison.OrdinalIgnoreCase))
        {
            return FailResult("Invalid Steam Login");
        }

        if (!queryParams.TryGetValue(SteamOpenId.ClaimedIdParam, out var claimedId) ||
            string.IsNullOrWhiteSpace(claimedId) ||
            !claimedId.StartsWith(SteamOpenId.SteamIdPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return FailResult("Invalid Claims");
        }

        var steamId = claimedId.Split('/').Last();

        if (!queryParams.TryGetValue(SteamOpenId.StateParam, out var rawState) || string.IsNullOrWhiteSpace(rawState))
        {
            return FailResult("Missing state");
        }

        StatePayload? state;

        try
        {
            state = JsonSerializer.Deserialize<StatePayload>(Uri.UnescapeDataString(rawState));
        }
        catch
        {
            return FailResult("Invalid state format");
        }

        if (state is null || string.IsNullOrWhiteSpace(state.LinkToken))
        {
            return FailResult("Invalid or missing link token");
        }

        var userId = await linkTokenService.ResolveTokenAsync(state.LinkToken);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return FailResult("Expired or invalid link token");
        }

        return Result.Success(new SteamCallbackPayload(userId, steamId));
    }

    private static Result<SteamCallbackPayload> FailResult(string message) =>
        Result.Failure<SteamCallbackPayload>(new ProblemError("SteamClient", message));
}