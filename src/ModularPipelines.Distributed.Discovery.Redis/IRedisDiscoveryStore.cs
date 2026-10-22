namespace ModularPipelines.Distributed.Discovery.Redis;

internal interface IRedisDiscoveryStore
{
    Task SetAsync(string key, string value, TimeSpan ttl, CancellationToken cancellationToken);

    Task<string?> GetAsync(string key, CancellationToken cancellationToken);

    Task<bool> RefreshAsync(string key, string expectedValue, TimeSpan ttl, CancellationToken cancellationToken);

    Task DeleteAsync(string key, string expectedValue, CancellationToken cancellationToken);
}
