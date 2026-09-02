using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Redis.Configuration;
using ModularPipelines.Distributed.Redis.Coordination;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.Artifacts;

/// <summary>
/// Factory that creates a <see cref="RedisDistributedArtifactStore"/> by connecting to Redis asynchronously.
/// Shares connection configuration with the coordinator when possible.
/// </summary>
internal sealed class RedisDistributedArtifactStoreFactory : IDistributedArtifactStoreFactory
{
    private readonly RedisDistributedOptions _redisOptions;
    private readonly ArtifactOptions _artifactOptions;
    private readonly DistributedOptions _distributedOptions;
    private readonly IConnectionMultiplexer _connection;

    public RedisDistributedArtifactStoreFactory(
        RedisDistributedOptions redisOptions,
        ArtifactOptions artifactOptions,
        IOptions<DistributedOptions> distributedOptions,
        IConnectionMultiplexer connection)
    {
        _redisOptions = redisOptions;
        _artifactOptions = artifactOptions;
        _distributedOptions = distributedOptions.Value;
        _connection = connection;
    }

    public Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken)
    {
        var database = _connection.GetDatabase();
        var keys = new RedisKeyBuilder(_redisOptions.KeyPrefix, _distributedOptions.RunId);
        IDistributedArtifactStore store = new RedisDistributedArtifactStore(database, keys, _artifactOptions);
        return Task.FromResult(store);
    }
}
