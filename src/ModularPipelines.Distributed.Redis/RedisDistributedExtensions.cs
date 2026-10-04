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

namespace ModularPipelines.Distributed.Redis;

/// <summary>
/// Extension methods for registering the Redis distributed coordinator, artifact store and module cache.
/// </summary>
/// <remarks>
/// The coordinator and artifact store share the unnamed <see cref="RedisOptions"/> but each opens its own
/// connection, so large artifact transfers do not queue ahead of coordinator commands on the client.
/// Both connections still share the network link and the Redis server, so size them for the combined load.
/// The module cache has its own <see cref="RedisOptions"/> and connection, so it can target another
/// Redis instance. Artifact settings that apply to every backend are configured through
/// <see cref="ArtifactOptions"/> and module cache settings through <see cref="ModuleCacheOptions"/>.
/// </remarks>
public static class RedisDistributedExtensions
{
    private const string ModuleCacheOptionsName = "ModularPipelines.RedisModuleCache";

    /// <summary>
    /// Keys the artifact store's connection. Artifact chunks are large, so they get their own
    /// multiplexer and do not queue ahead of the coordinator's heartbeats and lease renewals on the client.
    /// </summary>
    internal const string ArtifactConnectionKey = "ModularPipelines.Distributed.Redis.Artifacts";

    /// <summary>
    /// Enables a shareable Redis-backed module cache without enabling distributed execution.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configure">Configures the Redis connection, key prefix, chunking and expiry.</param>
    /// <param name="configureCache">Optionally transforms the immutable <see cref="ModuleCacheOptions"/>.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddRedisModuleCache(
        this PipelineBuilder builder,
        Action<RedisOptions> configure,
        Func<ModuleCacheOptions, ModuleCacheOptions>? configureCache = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);
        builder.Services.Configure(ModuleCacheOptionsName, configure);
        return AddRedisModuleCacheServices(builder, configureCache);
    }

    /// <summary>
    /// Enables a shareable Redis-backed module cache from configuration without enabling distributed execution.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="section">The configuration section bound to <see cref="RedisOptions"/>.</param>
    /// <param name="configureCache">Optionally transforms the immutable <see cref="ModuleCacheOptions"/>.</param>
    /// <returns>The same builder instance for chaining.</returns>
    [RequiresUnreferencedCode("Configuration binding requires members of RedisOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddRedisModuleCache(
        this PipelineBuilder builder,
        IConfigurationSection section,
        Func<ModuleCacheOptions, ModuleCacheOptions>? configureCache = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(section);
        builder.Services.Configure<RedisOptions>(ModuleCacheOptionsName, section);
        return AddRedisModuleCacheServices(builder, configureCache);
    }

    /// <summary>
    /// Registers the Redis-based distributed coordinator.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configure">Configures the Redis connection, key prefix and expiry.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddRedisDistributedCoordinator(
        this PipelineBuilder builder,
        Action<RedisOptions> configure)
    {
        ConfigureRedis(builder, configure);
        return AddRedisDistributedCoordinatorServices(builder);
    }

    /// <summary>
    /// Registers the Redis-based distributed coordinator from configuration.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="section">The configuration section bound to <see cref="RedisOptions"/>.</param>
    /// <returns>The same builder instance for chaining.</returns>
    [RequiresUnreferencedCode("Configuration binding requires members of RedisOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddRedisDistributedCoordinator(
        this PipelineBuilder builder,
        IConfigurationSection section)
    {
        ConfigureRedis(builder, section);
        return AddRedisDistributedCoordinatorServices(builder);
    }

    /// <summary>
    /// Registers the Redis-based distributed artifact store.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configure">Configures the Redis connection, key prefix, chunking and expiry.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddRedisDistributedArtifactStore(
        this PipelineBuilder builder,
        Action<RedisOptions> configure)
    {
        ConfigureRedis(builder, configure);
        return AddRedisDistributedArtifactStoreServices(builder);
    }

    /// <summary>
    /// Registers the Redis-based distributed artifact store from configuration.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="section">The configuration section bound to <see cref="RedisOptions"/>.</param>
    /// <returns>The same builder instance for chaining.</returns>
    [RequiresUnreferencedCode("Configuration binding requires members of RedisOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddRedisDistributedArtifactStore(
        this PipelineBuilder builder,
        IConfigurationSection section)
    {
        ConfigureRedis(builder, section);
        return AddRedisDistributedArtifactStoreServices(builder);
    }

    /// <summary>
    /// Registers both the Redis-based coordinator and artifact store with shared options. The artifact
    /// store opens its own connection so large transfers do not queue ahead of coordinator commands.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configure">Configures the Redis connection, key prefix, chunking and expiry.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddRedisDistributed(
        this PipelineBuilder builder,
        Action<RedisOptions> configure)
    {
        ConfigureRedis(builder, configure);
        AddRedisDistributedCoordinatorServices(builder);
        return AddRedisDistributedArtifactStoreServices(builder);
    }

    /// <summary>
    /// Registers both the Redis-based coordinator and artifact store from configuration with shared
    /// options. The artifact store opens its own connection so large transfers do not queue ahead of
    /// coordinator commands.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="section">The configuration section bound to <see cref="RedisOptions"/>.</param>
    /// <returns>The same builder instance for chaining.</returns>
    [RequiresUnreferencedCode("Configuration binding requires members of RedisOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddRedisDistributed(
        this PipelineBuilder builder,
        IConfigurationSection section)
    {
        ConfigureRedis(builder, section);
        AddRedisDistributedCoordinatorServices(builder);
        return AddRedisDistributedArtifactStoreServices(builder);
    }

    private static void ConfigureRedis(PipelineBuilder builder, Action<RedisOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);
        builder.Services.Configure(configure);
        AddDistributedConnection(builder);
    }

    [RequiresUnreferencedCode("Configuration binding requires members of RedisOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    private static void ConfigureRedis(PipelineBuilder builder, IConfigurationSection section)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(section);
        builder.Services.Configure<RedisOptions>(section);
        AddDistributedConnection(builder);
    }

    private static void AddDistributedConnection(PipelineBuilder builder)
    {
        builder.RequireExplicitRunId();
        AddValidation(builder.Services, Microsoft.Extensions.Options.Options.DefaultName);

        // An internal type, so neither another package nor the application can replace or
        // accidentally share this connection. It connects on first use, asynchronously.
        builder.Services.TryAddSingleton(serviceProvider =>
            new RedisConnectionProvider(serviceProvider.GetRequiredService<IOptions<RedisOptions>>().Value));
        builder.Services.TryAddKeyedSingleton(
            ArtifactConnectionKey,
            (serviceProvider, _) =>
                new RedisConnectionProvider(serviceProvider.GetRequiredService<IOptions<RedisOptions>>().Value));
    }

    private static void AddValidation(IServiceCollection services, string name)
    {
        // A cache-only registration must not validate the unused unnamed options when
        // core services enumerate options, for example when discovering secrets.
        services.AddSingleton<IValidateOptions<RedisOptions>>(serviceProvider =>
            new RedisOptionsValidator(name, serviceProvider.GetRequiredService<IOptions<DistributedOptions>>()));
        services.AddOptions<RedisOptions>(name).ValidateOnStart();
    }

    private static PipelineBuilder AddRedisDistributedCoordinatorServices(PipelineBuilder builder) =>
        builder.AddDistributedCoordinatorFactory<RedisDistributedCoordinatorFactory>();

    private static PipelineBuilder AddRedisDistributedArtifactStoreServices(PipelineBuilder builder) =>
        builder.AddDistributedArtifactStoreFactory<RedisDistributedArtifactStoreFactory>();

    private static PipelineBuilder AddRedisModuleCacheServices(
        PipelineBuilder builder,
        Func<ModuleCacheOptions, ModuleCacheOptions>? configureCache)
    {
        AddValidation(builder.Services, ModuleCacheOptionsName);
        builder.Services.TryAddKeyedSingleton(
            ModuleCacheOptionsName,
            (serviceProvider, _) => new RedisConnectionProvider(
                serviceProvider.GetRequiredService<IOptionsMonitor<RedisOptions>>().Get(ModuleCacheOptionsName)));
        builder.Services.TryAddSingleton(serviceProvider => new RedisModuleCache(
            serviceProvider.GetRequiredKeyedService<RedisConnectionProvider>(ModuleCacheOptionsName),
            serviceProvider.GetRequiredService<IOptionsMonitor<RedisOptions>>().Get(ModuleCacheOptionsName),
            serviceProvider.GetRequiredService<IOptions<ModuleCacheOptions>>().Value));

        return builder.AddModuleCache<RedisModuleCache>(configureCache);
    }
}
