using Modules.ExternalAccounts.Application.Common.Contracts.Services;
using StackExchange.Redis;

namespace Modules.ExternalAccounts.Infrastructure.Services;

public class RedisLinkTokenService(IConnectionMultiplexer redis) : ILinkTokenService
{
    private readonly IDatabase _db = redis.GetDatabase();

    private readonly TimeSpan _defaultTtl = TimeSpan.FromMinutes(5);

    public async Task<string> CreateTokenAsync(string userId, TimeSpan? ttl = null)
    {
        var token = Guid.NewGuid().ToString("N");
        
        var expiry = ttl ?? _defaultTtl;

        await _db.StringSetAsync($"link:{token}", userId, expiry);

        return token;
    }

    public async Task<string?> ResolveTokenAsync(string token)
    {
        var value = await _db.StringGetAsync($"link:{token}");
        
        return value.HasValue ? value.ToString() : null;
    }
}