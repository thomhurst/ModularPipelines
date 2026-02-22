using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Distributed.Redis.Artifacts;
using ModularPipelines.Distributed.Redis.Configuration;
using ModularPipelines.Distributed.Redis.Coordination;

namespace ModularPipelines.Distributed.Redis.Extensions;

/// <summary>
/// Extension methods for registering the Redis distributed coordinator and artifact store.
/// </summary>
public static class RedisDistributedExtensions
{
    /// <summary>
    /// Registers the Redis-based distributed coordinator factory.
    /// Must be called after <c>AddDistributedMode</c>.
    /// </summary>
    public static PipelineBuilder AddRedisDistributedCoordinator(
        this PipelineBuilder builder,
        Action<RedisDistributedOptions> configure)
    {
        var options = new RedisDistributedOptions();
        configure(options);

        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton<IDistributedCoordinatorFactory, RedisDistributedCoordinatorFactory>();

        return builder;
    }

    /// <summary>
    /// Registers the Redis-based distributed artifact store factory.
    /// Must be called after <c>AddDistributedMode</c>.
    /// </summary>
    public static PipelineBuilder AddRedisDistributedArtifactStore(
        this PipelineBuilder builder,
        Action<ArtifactOptions>? configure = null)
    {
        var options = new ArtifactOptions();
        configure?.Invoke(options);

        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton<IDistributedArtifactStoreFactory, RedisDistributedArtifactStoreFactory>();

        return builder;
    }

    /// <summary>
    /// Registers both Redis-based coordinator and artifact store.
    /// Convenience method for using Redis for both orchestration and artifacts.
    /// </summary>
    public static PipelineBuilder AddRedisDistributed(
        this PipelineBuilder builder,
        Action<RedisDistributedOptions> configureRedis,
        Action<ArtifactOptions>? configureArtifacts = null)
    {
        builder.AddRedisDistributedCoordinator(configureRedis);
        builder.AddRedisDistributedArtifactStore(configureArtifacts);
        return builder;
    }
}
