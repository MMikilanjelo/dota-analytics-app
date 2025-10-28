using Common.Abstractions;

namespace Modules.ExternalAccounts.Domain.ValueObjects;

public sealed class ProviderKind : Enumeration
{
    public static readonly ProviderKind Steam = new(1, "Steam");

    private ProviderKind(int value, string name) : base(value, name)
    {
    }
}