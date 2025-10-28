namespace Modules.ExternalAccounts.Infrastructure.Extensions;

public static class ExternalAccountEndpoints
{
    public static class Steam
    {
        public const string OpenIdUrl = "https://steamcommunity.com/openid/login";
        public const string OpenIdNs = "http://specs.openid.net/auth/2.0";
        public const string IdentifierSelect = "http://specs.openid.net/auth/2.0/identifier_select";
    }
}