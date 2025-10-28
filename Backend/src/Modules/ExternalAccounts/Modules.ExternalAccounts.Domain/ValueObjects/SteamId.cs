using System.Dynamic;
using System.Text.RegularExpressions;
using Common.Abstractions;

namespace Modules.ExternalAccounts.Domain.ValueObjects;

public class SteamId : ExternalAccountId
{
    private SteamId(string value) : base(value , ProviderKind.Steam)
    {
    }

    public static SteamId Create(string value)
    {
        if (!Regex.IsMatch(value, @"^\d{17}$"))
        {
            throw new ArgumentException("Invalid Steam ID format");
        }

        return new SteamId(value);
    }
}