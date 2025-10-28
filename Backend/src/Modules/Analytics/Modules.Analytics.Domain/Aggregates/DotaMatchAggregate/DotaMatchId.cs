namespace Modules.Analytics.Domain.Aggregates.DotaMatchAggregate;

public readonly record struct DotaMatchId(long Value)
{
    public static DotaMatchId Empty => new(0);
    public override string ToString() => Value.ToString();

    public static implicit operator long(DotaMatchId id) => id.Value;
    public static implicit operator DotaMatchId(long value) => new(value);
}