using SharedKernel;

namespace Modules.ExternalAccounts.Application.Common.Contracts.Clients;

public interface ISteamClient
{
    Task<string> BuildRedirectToLoginUrl(string scheme,
        string host,
        string pathBase,
        string redirectUri,
        string userId);

    Task<Result<SteamCallbackPayload>> ValidateCallbackAsync(
        IDictionary<string, string> queryParams,
        CancellationToken cancellationToken
    );
}