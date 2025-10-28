using Microsoft.Extensions.Logging;
using Modules.Analytics.Domain.Aggregates.DotaMatchAggregate;
using Modules.Analytics.Infrastructure.Services.OpenDotaClient;
using Modules.Analytics.Infrastructure.Services.OpenDotaClient.Models;
using Quartz;

namespace Modules.Analytics.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public sealed class FetchDotaMatchesJob(
    ILogger<FetchDotaMatchesJob> logger,
    IDotaMatchRepository dotaMatchRepository,
    IOpenDotaService openDotaClient) : IJob
{
    public static readonly JobKey Key = JobKey.Create(nameof(FetchDotaMatchesJob));


    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Running FetchDotaMatchesJob,at {Time}", DateTime.UtcNow);

        try
        {
            long? lastMatchId = null;

            var fetchedMatches = await openDotaClient.GetRecentMatchIdsAsync(75, 85, lastMatchId);

            if (fetchedMatches.Count == 0)
            {
                logger.LogInformation("No matches found,at {Time}", DateTime.UtcNow);
                return;
            }

            var queue = new Queue<PublicMatch>(fetchedMatches);

            while (queue.Count > 0)
            {
                var match = queue.Dequeue();

                long? matchId = match?.MatchId;

                if (!matchId.HasValue) continue;

                if (await dotaMatchRepository.MatchExists(matchId.Value))
                {
                    logger.LogInformation("Match {MatchId} already processed ,at {Time}", matchId.Value, DateTime.UtcNow);
                    continue;
                }

                var details = await openDotaClient.GetMatchDetailsAsync(matchId.Value);

                await dotaMatchRepository.AddDotaMatch(matchId.Value, details);

                await Task.Delay(TimeSpan.FromSeconds(2));

                logger.LogInformation("Saved match {MatchId}", matchId);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching Dota matches");
        }
    }
}