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

    public RedisDistributedArtifactStoreFactory(
        RedisDistributedOptions redisOptions,
        ArtifactOptions artifactOptions)
    {
        _redisOptions = redisOptions;
        _artifactOptions = artifactOptions;
    }

    public async Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken)
    {
        var connection = await ConnectionMultiplexer.ConnectAsync(_redisOptions.ConnectionString);
        var database = connection.GetDatabase();
        var runId = RunIdentifierResolver.Resolve(_redisOptions.RunIdentifier);
        var keys = new RedisKeyBuilder(_redisOptions.KeyPrefix, runId);
        return new RedisDistributedArtifactStore(database, keys, _artifactOptions);
    }
}
