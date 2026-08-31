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
        var options = ConfigureRedis(builder, configure);
        options.RunIdentifier = RunIdentifierResolver.ResolveRunIdentifier(options.RunIdentifier)
            ?? throw new InvalidOperationException(
                "Redis distributed coordination requires a unique RunIdentifier for each pipeline execution. "
                + "Configure RunIdentifier explicitly or provide a supported CI run identifier.");

        builder.Services.PostConfigure<DistributedOptions>(distributedOptions =>
            distributedOptions.RunIdentifier ??= options.RunIdentifier);
        builder.Services.AddSingleton<IDistributedCoordinatorFactory, RedisDistributedCoordinatorFactory>();

        return builder;
    }

    /// <summary>
    /// Registers the Redis-based distributed artifact store factory.
    /// Must be called after <c>AddDistributedMode</c>.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configureRedis">Configures the Redis connection and key prefix.</param>
    /// <param name="configureArtifacts">Optionally configures cache chunking and expiry.</param>
    /// <returns></returns>
    public static PipelineBuilder AddRedisDistributedArtifactStore(
        this PipelineBuilder builder,
        Action<RedisDistributedOptions> configureRedis,
        Action<ArtifactOptions>? configureArtifacts = null)
    {
        ConfigureRedis(builder, configureRedis);

        return AddRedisDistributedArtifactStoreFactory(builder, configureArtifacts);
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
        return AddRedisDistributedArtifactStoreFactory(builder, configureArtifacts);
    }

    private static RedisDistributedOptions ConfigureRedis(
        PipelineBuilder builder,
        Action<RedisDistributedOptions> configure)
    {
        var registeredOptions = builder.Services
            .LastOrDefault(descriptor => descriptor.ServiceType == typeof(RedisDistributedOptions))
            ?.ImplementationInstance;

        if (registeredOptions is not RedisDistributedOptions options)
        {
            options = new RedisDistributedOptions();
            builder.Services.AddSingleton(options);
        }

        configure(options);
        builder.Services.TryAddSingleton<IConnectionMultiplexer>(serviceProvider =>
        {
            var redisOptions = serviceProvider.GetRequiredService<RedisDistributedOptions>();
            return ConnectionMultiplexer.Connect(redisOptions.ConnectionString);
        });

        return options;
    }

    private static PipelineBuilder AddRedisDistributedArtifactStoreFactory(
        PipelineBuilder builder,
        Action<ArtifactOptions>? configureArtifacts)
    {
        if (configureArtifacts is not null)
        {
            builder.Services.Configure(configureArtifacts);
        }

        return builder.AddDistributedArtifactStoreFactory<RedisDistributedArtifactStoreFactory>();
    }
}
