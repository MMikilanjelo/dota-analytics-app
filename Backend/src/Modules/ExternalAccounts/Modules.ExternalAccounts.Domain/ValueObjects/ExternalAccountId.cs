using System.Diagnostics.CodeAnalysis;
using Common.Abstractions;

namespace Modules.ExternalAccounts.Domain.ValueObjects;

public class ExternalAccountId : ValueObject
{
    public string Value { get; }
    public ProviderKind Kind { get; }

    protected ExternalAccountId(string value, ProviderKind kind)
    {
        Value = value;
        Kind = kind;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
        yield return Kind;
    }
}