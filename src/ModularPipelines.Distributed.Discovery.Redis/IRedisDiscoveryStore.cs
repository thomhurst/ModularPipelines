namespace ModularPipelines.Distributed.Discovery.Redis;

internal interface IRedisDiscoveryStore
{
    Task SetAsync(string key, string value, TimeSpan ttl, CancellationToken cancellationToken);

    Task<string?> GetAsync(string key, CancellationToken cancellationToken);
}
