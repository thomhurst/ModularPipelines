using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Redis.Coordination;

namespace ModularPipelines.Distributed.Redis.Artifacts;

/// <summary>
/// Factory that creates a <see cref="RedisDistributedArtifactStore"/> by connecting to Redis asynchronously.
/// Shares the package's distributed connection with the coordinator.
/// </summary>
internal sealed class RedisDistributedArtifactStoreFactory(
    IOptions<RedisOptions> redisOptions,
    IOptions<DistributedOptions> distributedOptions,
    RedisConnectionProvider connections) : IDistributedArtifactStoreFactory
{
    public async Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken)
    {
        var connection = await connections.GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        var options = redisOptions.Value;
        var keys = new RedisKeyBuilder(options.KeyPrefix, distributedOptions.Value.RunId);
        return new RedisDistributedArtifactStore(connection.GetDatabase(), keys, options);
    }
}
