using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Redis.Coordination;

namespace ModularPipelines.Distributed.Redis.Artifacts;

/// <summary>
/// Factory that creates a <see cref="RedisDistributedArtifactStore"/> by connecting to Redis asynchronously.
/// Uses the same <see cref="RedisOptions"/> as the coordinator but its own connection, so large
/// artifact transfers do not queue ahead of coordinator commands on the client. Both connections
/// still share the network link and the Redis server.
/// </summary>
internal sealed class RedisDistributedArtifactStoreFactory(
    IOptions<RedisOptions> redisOptions,
    IOptions<DistributedOptions> distributedOptions,
    [FromKeyedServices(RedisDistributedExtensions.ArtifactConnectionKey)] RedisConnectionProvider connections)
    : IDistributedArtifactStoreFactory
{
    public async Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken)
    {
        var connection = await connections.GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        var options = redisOptions.Value;
        var keys = new RedisKeyBuilder(options.KeyPrefix, distributedOptions.Value.RunId);
        return new RedisDistributedArtifactStore(connection.GetDatabase(), keys, options);
    }
}
