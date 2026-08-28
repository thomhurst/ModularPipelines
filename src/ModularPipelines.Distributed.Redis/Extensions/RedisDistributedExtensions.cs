using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Distributed.Redis.Artifacts;
using ModularPipelines.Distributed.Redis.Caching;
using ModularPipelines.Distributed.Redis.Configuration;
using ModularPipelines.Distributed.Redis.Coordination;
using ModularPipelines.Extensions;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.Extensions;

/// <summary>
/// Extension methods for registering the Redis distributed coordinator and artifact store.
/// </summary>
public static class RedisDistributedExtensions
{
    private static readonly object ModuleCacheConnectionKey = new();

    /// <summary>
    /// Enables a shareable Redis-backed module cache without enabling distributed execution.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configureRedis">Configures the Redis connection and key prefix.</param>
    /// <param name="configureArtifacts">Optionally configures cache chunking and expiry.</param>
    /// <param name="configureCache">Optionally configures fingerprinting behavior.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddRedisModuleCache(
        this PipelineBuilder builder,
        Action<RedisDistributedOptions> configureRedis,
        Action<ArtifactOptions>? configureArtifacts = null,
        Action<ModuleCacheOptions>? configureCache = null)
    {
        var redisOptions = new RedisDistributedOptions();
        configureRedis(redisOptions);
        var artifactOptions = new ArtifactOptions();
        configureArtifacts?.Invoke(artifactOptions);

        builder.Services.TryAddKeyedSingleton<IConnectionMultiplexer>(
            ModuleCacheConnectionKey,
            (_, _) => ConnectionMultiplexer.Connect(redisOptions.ConnectionString));
        builder.Services.TryAddSingleton(serviceProvider => new RedisModuleCache(
            serviceProvider.GetRequiredKeyedService<IConnectionMultiplexer>(ModuleCacheConnectionKey),
            redisOptions,
            artifactOptions,
            serviceProvider.GetRequiredService<IOptions<ModuleCacheOptions>>().Value));

        return builder.AddModuleCache<RedisModuleCache>(configureCache);
    }

    /// <summary>
    /// Registers the Redis-based distributed coordinator factory.
    /// Must be called after <c>AddDistributedMode</c>.
    /// </summary>
    /// <returns></returns>
    public static PipelineBuilder AddRedisDistributedCoordinator(
        this PipelineBuilder builder,
        Action<RedisDistributedOptions> configure)
    {
        var options = new RedisDistributedOptions();
        configure(options);
        options.RunIdentifier = RunIdentifierResolver.ResolveRunIdentifier(options.RunIdentifier)
            ?? throw new InvalidOperationException(
                "Redis distributed coordination requires a unique RunIdentifier for each pipeline execution. "
                + "Configure RunIdentifier explicitly or provide a supported CI run identifier.");

        builder.Services.AddSingleton(options);
        builder.Services.PostConfigure<DistributedOptions>(distributedOptions =>
            distributedOptions.RunIdentifier ??= options.RunIdentifier);
        builder.Services.TryAddSingleton<IConnectionMultiplexer>(sp =>
        {
            var opts = sp.GetRequiredService<RedisDistributedOptions>();
            return ConnectionMultiplexer.Connect(opts.ConnectionString);
        });
        builder.Services.AddSingleton<IDistributedCoordinatorFactory, RedisDistributedCoordinatorFactory>();

        return builder;
    }

    /// <summary>
    /// Registers the Redis-based distributed artifact store factory.
    /// Must be called after <c>AddDistributedMode</c>.
    /// </summary>
    /// <returns></returns>
    public static PipelineBuilder AddRedisDistributedArtifactStore(
        this PipelineBuilder builder,
        Action<ArtifactOptions>? configure = null)
    {
        var options = new ArtifactOptions();
        configure?.Invoke(options);

        builder.Services.AddSingleton(options);
        return builder.AddDistributedArtifactStoreFactory<RedisDistributedArtifactStoreFactory>();
    }

    /// <summary>
    /// Registers both Redis-based coordinator and artifact store.
    /// Convenience method for using Redis for both orchestration and artifacts.
    /// </summary>
    /// <returns></returns>
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
