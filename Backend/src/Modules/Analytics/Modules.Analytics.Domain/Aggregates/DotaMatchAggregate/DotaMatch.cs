namespace Modules.Analytics.Domain.Aggregates.DotaMatchAggregate;

public class DotaMatch
{
    public DotaMatchId Id { get; set; }
    public string RawJson { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}