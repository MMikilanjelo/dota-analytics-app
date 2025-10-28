using Microsoft.Extensions.Options;
using Modules.Analytics.Domain.Aggregates.DotaMatchAggregate;
using Modules.Analytics.Infrastructure.Options;
using MongoDB.Driver;

namespace Modules.Analytics.Infrastructure.Data.Repositories;

public class DotaMatchRepository : IDotaMatchRepository
{
    private readonly IMongoCollection<DotaMatch> _collection;

    public DotaMatchRepository(IOptions<MongoDbOptions> configuration)
    {
        var client = new MongoClient(configuration.Value.ConnectionString);
        var database = client.GetDatabase(configuration.Value.DatabaseName);
        _collection = database.GetCollection<DotaMatch>(nameof(DotaMatch));
    }

    public async Task AddDotaMatch(long matchId, string rawJson)
    {
        var doc = new DotaMatch
        {
            Id = new DotaMatchId(matchId),
            RawJson = rawJson,
            CreatedAt = DateTime.UtcNow
        };

        await _collection.InsertOneAsync(doc);
    }

    public async Task<bool> MatchExists(DotaMatchId matchId)
    {
        var filter = Builders<DotaMatch>.Filter.Eq(m => m.Id, matchId);
        var count = await _collection.CountDocumentsAsync(filter);
        return count > 0;
    }
}