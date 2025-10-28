namespace Modules.Analytics.Domain.Aggregates.DotaMatchAggregate;

public interface IDotaMatchRepository
{
    Task AddDotaMatch(long matchId, string rawJson);
    Task<bool> MatchExists(DotaMatchId dotaMatchId);
}