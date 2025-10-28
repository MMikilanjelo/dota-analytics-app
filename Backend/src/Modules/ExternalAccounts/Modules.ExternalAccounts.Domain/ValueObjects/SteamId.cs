using System.Dynamic;
using System.Text.RegularExpressions;
using SharedKernel;
using SharedKernel.Abstractions;
using SharedKernel.Errors;

namespace Modules.ExternalAccounts.Domain.ValueObjects;

public class SteamId : ExternalAccountId
{
    private SteamId(string value) : base(value, ProviderKind.Steam)
    {
    }

    public static Result<SteamId> Create(string value)
    {
        if (!Regex.IsMatch(value, @"^\d{17}$"))
        {
            return Result.Failure<SteamId>(new ValidationError([new InvalidValueFormatError()]));
        }

        return new SteamId(value);
    }
}