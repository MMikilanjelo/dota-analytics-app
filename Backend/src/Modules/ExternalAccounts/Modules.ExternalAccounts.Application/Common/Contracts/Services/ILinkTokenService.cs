namespace Modules.ExternalAccounts.Application.Common.Contracts.Services;

public interface ILinkTokenService
{
    Task<string> CreateTokenAsync(string userId, TimeSpan? ttl = null);

    Task<string?> ResolveTokenAsync(string token);
}