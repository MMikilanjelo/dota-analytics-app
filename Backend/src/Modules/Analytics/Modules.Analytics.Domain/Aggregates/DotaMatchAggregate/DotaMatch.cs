namespace Modules.Analytics.Domain.Aggregates.DotaMatchAggregate;

public class DotaMatch
{
    public DotaMatchId Id { get; init; }
    public string RawJson { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}