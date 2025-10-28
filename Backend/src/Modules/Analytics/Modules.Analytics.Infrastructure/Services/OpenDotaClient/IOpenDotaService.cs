using Modules.Analytics.Infrastructure.Services.OpenDotaClient.Models;

namespace Modules.Analytics.Infrastructure.Services.OpenDotaClient;

public interface IOpenDotaService
{
    Task<List<PublicMatch>> GetRecentMatchIdsAsync(int minRank, int maxRank, long? lastMatchId = null);
    Task<string> GetMatchDetailsAsync(long matchId);
}