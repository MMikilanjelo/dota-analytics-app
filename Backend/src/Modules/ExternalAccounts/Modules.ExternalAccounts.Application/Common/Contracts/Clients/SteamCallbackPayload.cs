namespace Modules.ExternalAccounts.Application.Common.Contracts.Clients;

public sealed record SteamCallbackPayload(string UserId, string SteamId);