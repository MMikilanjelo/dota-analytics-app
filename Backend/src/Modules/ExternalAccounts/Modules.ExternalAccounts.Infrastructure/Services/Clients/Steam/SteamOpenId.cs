namespace Modules.ExternalAccounts.Infrastructure.Services.Clients.Steam;

public static class SteamOpenId
{
    public const string Url = "https://steamcommunity.com/openid/login";

    public const string Ns = "http://specs.openid.net/auth/2.0";
    public const string IdentifierSelect = "http://specs.openid.net/auth/2.0/identifier_select";

    public const string NsParam = "openid.ns";
    public const string ModeParam = "openid.mode";
    public const string ReturnToParam = "openid.return_to";
    public const string RealmParam = "openid.realm";
    public const string IdentityParam = "openid.identity";
    public const string ClaimedIdParam = "openid.claimed_id";
    public const string StateParam = "state";

    public const string ModeCheckId = "checkid_setup";
    public const string ModeCheckAuth = "check_authentication";

    public const string ValidResponse = "is_valid:true";

    public const string SteamIdPrefix = "https://steamcommunity.com/openid/id/";
}