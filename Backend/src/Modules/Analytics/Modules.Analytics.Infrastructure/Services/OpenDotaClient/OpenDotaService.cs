using Modules.Analytics.Infrastructure.Services.OpenDotaClient.Models;
using Newtonsoft.Json;

namespace Modules.Analytics.Infrastructure.Services.OpenDotaClient;

public class OpenDotaService(HttpClient httpClient) : IOpenDotaService
{
    public async Task<List<PublicMatch>> GetRecentMatchIdsAsync(int minRank, int maxRank, long? lastMatchId = null)
    {
        var queryParams = new List<string>
        {
            $"min_rank={minRank}",
            $"max_rank={maxRank}"
        };

        if (lastMatchId.HasValue)
            queryParams.Add($"less_than_match_id={lastMatchId.Value}");

        var response = await GetRequestResponseMessageAsync("publicMatches", queryParams);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var matches = JsonConvert.DeserializeObject<List<PublicMatch>>(json);

        return matches ?? [];
    }

    public async Task<string> GetMatchDetailsAsync(long matchId)
    {
        string url = $"matches/{matchId}";

        var response = await httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    private async Task<HttpResponseMessage> GetRequestResponseMessageAsync(string url, IList<string>? queryParameters = null)
    {
        queryParameters ??= new List<string>();

        var argumentsString = string.Join("&", queryParameters.Where(arg => !string.IsNullOrEmpty(arg)));

        var fullUrl = $@"{url}?{argumentsString}";

        var message = await httpClient.GetAsync(fullUrl);

        return message;
    }
}