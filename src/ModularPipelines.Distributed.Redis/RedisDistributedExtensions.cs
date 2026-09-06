using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Distributed.Redis.Artifacts;
using ModularPipelines.Distributed.Redis.Caching;
using ModularPipelines.Distributed.Redis.Coordination;
using ModularPipelines.Extensions;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis;

/// <summary>
/// Extension methods for registering the Redis distributed coordinator and artifact store.
/// </summary>
public static class RedisDistributedExtensions
{
    private const string ModuleCacheOptionsName = "ModularPipelines.RedisModuleCache";
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
        builder.Services.Configure(ModuleCacheOptionsName, configureRedis);
        if (configureArtifacts is not null)
        {
            builder.Services.Configure(ModuleCacheOptionsName, configureArtifacts);
        }

        return AddRedisModuleCacheServices(builder, configureCache);
    }

    /// <summary>
    /// Enables a shareable Redis-backed module cache from configuration without enabling distributed execution.
    /// </summary>
    [RequiresUnreferencedCode("Configuration binding requires members of RedisDistributedOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddRedisModuleCache(
        this PipelineBuilder builder,
        IConfigurationSection redisSection,
        Action<ArtifactOptions>? configureArtifacts = null,
        Action<ModuleCacheOptions>? configureCache = null)
    {
        builder.Services.Configure<RedisDistributedOptions>(ModuleCacheOptionsName, redisSection);
        if (configureArtifacts is not null)
        {
            builder.Services.Configure(ModuleCacheOptionsName, configureArtifacts);
        }

        return AddRedisModuleCacheServices(builder, configureCache);
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
        ConfigureRedis(builder, configure);
        return AddRedisDistributedCoordinatorServices(builder);
    }

    /// <summary>
    /// Registers the Redis-based distributed coordinator factory from configuration.
    /// Must be called after <c>AddDistributedMode</c>.
    /// </summary>
    [RequiresUnreferencedCode("Configuration binding requires members of RedisDistributedOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddRedisDistributedCoordinator(
        this PipelineBuilder builder,
        IConfigurationSection section)
    {
        ConfigureRedis(builder, section);
        return AddRedisDistributedCoordinatorServices(builder);
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
    /// Registers the Redis-based distributed artifact store factory from configuration.
    /// Must be called after <c>AddDistributedMode</c>.
    /// </summary>
    [RequiresUnreferencedCode("Configuration binding requires members of RedisDistributedOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddRedisDistributedArtifactStore(
        this PipelineBuilder builder,
        IConfigurationSection redisSection,
        Action<ArtifactOptions>? configureArtifacts = null)
    {
        ConfigureRedis(builder, redisSection);
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

    /// <summary>
    /// Registers both Redis-based coordinator and artifact store from configuration.
    /// </summary>
    [RequiresUnreferencedCode("Configuration binding requires members of RedisDistributedOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddRedisDistributed(
        this PipelineBuilder builder,
        IConfigurationSection redisSection,
        Action<ArtifactOptions>? configureArtifacts = null)
    {
        builder.AddRedisDistributedCoordinator(redisSection);
        return AddRedisDistributedArtifactStoreFactory(builder, configureArtifacts);
    }

    private static void ConfigureRedis(
        PipelineBuilder builder,
        Action<RedisDistributedOptions> configure)
    {
        builder.Services.Configure(configure);
        AddRedisConnection(builder);
    }

    private static void ConfigureRedis(PipelineBuilder builder, IConfigurationSection section)
    {
        builder.Services.Configure<RedisDistributedOptions>(section);
        AddRedisConnection(builder);
    }

    private static void AddRedisConnection(PipelineBuilder builder)
    {
        builder.Services.TryAddSingleton<IConnectionMultiplexer>(serviceProvider =>
        {
            var redisOptions = serviceProvider.GetRequiredService<IOptions<RedisDistributedOptions>>().Value;
            return ConnectionMultiplexer.Connect(redisOptions.ConnectionString);
        });
    }

    private static PipelineBuilder AddRedisDistributedCoordinatorServices(PipelineBuilder builder)
    {
        builder.RequireExplicitRunId();
        builder.Services.AddSingleton<IDistributedCoordinatorFactory, RedisDistributedCoordinatorFactory>();

        return builder;
    }

    private static PipelineBuilder AddRedisModuleCacheServices(
        PipelineBuilder builder,
        Action<ModuleCacheOptions>? configureCache)
    {
        builder.Services.TryAddKeyedSingleton<IConnectionMultiplexer>(
            ModuleCacheConnectionKey,
            (serviceProvider, _) => ConnectionMultiplexer.Connect(
                serviceProvider.GetRequiredService<IOptionsMonitor<RedisDistributedOptions>>()
                    .Get(ModuleCacheOptionsName).ConnectionString));
        builder.Services.TryAddSingleton(serviceProvider => new RedisModuleCache(
            serviceProvider.GetRequiredKeyedService<IConnectionMultiplexer>(ModuleCacheConnectionKey),
            serviceProvider.GetRequiredService<IOptionsMonitor<RedisDistributedOptions>>()
                .Get(ModuleCacheOptionsName),
            serviceProvider.GetRequiredService<IOptionsMonitor<ArtifactOptions>>()
                .Get(ModuleCacheOptionsName),
            serviceProvider.GetRequiredService<IOptions<ModuleCacheOptions>>().Value));

        return builder.AddModuleCache<RedisModuleCache>(configureCache);
    }

    private static PipelineBuilder AddRedisDistributedArtifactStoreFactory(
        PipelineBuilder builder,
        Action<ArtifactOptions>? configureArtifacts)
    {
        builder.RequireExplicitRunId();
        if (configureArtifacts is not null)
        {
            builder.Services.Configure(configureArtifacts);
        }

        return builder.AddDistributedArtifactStoreFactory<RedisDistributedArtifactStoreFactory>();
    }
}
