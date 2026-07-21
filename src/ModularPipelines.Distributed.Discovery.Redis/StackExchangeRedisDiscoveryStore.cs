using StackExchange.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis;

internal sealed class StackExchangeRedisDiscoveryStore(IConnectionMultiplexer connection) : IRedisDiscoveryStore
{
    public async Task SetAsync(
        string key,
        string value,
        TimeSpan ttl,
        CancellationToken cancellationToken)
    {
        await connection.GetDatabase().StringSetAsync(key, value, ttl);
    }

    public async Task<string?> GetAsync(string key, CancellationToken cancellationToken)
    {
        var value = await connection.GetDatabase().StringGetAsync(key);
        return value.HasValue ? value.ToString() : null;
    }
}
