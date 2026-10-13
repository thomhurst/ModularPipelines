using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Extension methods for registering Redis-based master endpoint discovery.
/// </summary>
public static class RedisDiscoveryExtensions
{
    /// <summary>
    /// Registers Redis-based master endpoint discovery. Options are validated at startup.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configure">Transforms the immutable Redis discovery options.</param>
    /// <returns>The pipeline builder for chaining.</returns>
    public static PipelineBuilder AddRedisMasterDiscovery(
        this PipelineBuilder builder,
        Func<RedisDiscoveryOptions, RedisDiscoveryOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        TransformOptionsFactory<RedisDiscoveryOptions>.Configure(builder.Services, configure);
        return AddRedisMasterDiscoveryServices(builder);
    }

    /// <summary>
    /// Registers Redis-based master endpoint discovery from configuration. Options are validated at startup.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="section">The configuration section containing Redis discovery options.</param>
    /// <returns>The pipeline builder for chaining.</returns>
    [RequiresUnreferencedCode("Configuration binding requires members of RedisDiscoveryOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddRedisMasterDiscovery(
        this PipelineBuilder builder,
        IConfigurationSection section)
    {
        TransformOptionsFactory<RedisDiscoveryOptions>.Register(builder.Services);
        builder.Services.Configure<RedisDiscoveryOptions>(section);
        return AddRedisMasterDiscoveryServices(builder);
    }

    private static PipelineBuilder AddRedisMasterDiscoveryServices(PipelineBuilder builder)
    {
        builder.RequireExplicitRunId();
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IValidateOptions<RedisDiscoveryOptions>, RedisDiscoveryOptionsValidator>());
        builder.Services.AddOptions<RedisDiscoveryOptions>().ValidateOnStart();

        // Discovery owns its connection so it neither replaces nor depends on an application
        // IConnectionMultiplexer, and connects lazily without blocking service-provider construction.
        builder.Services.TryAddSingleton<IRedisDiscoveryStore>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RedisDiscoveryOptions>>().Value;
            return options.RestUrl is { } restUrl
                ? new RestRedisDiscoveryStore(restUrl, options.RestToken!)
                : new StackExchangeRedisDiscoveryStore(options);
        });
        builder.Services.TryAddSingleton<IMasterDiscovery>(sp => new RedisMasterDiscovery(
            sp.GetRequiredService<IRedisDiscoveryStore>(),
            sp.GetRequiredService<IOptions<RedisDiscoveryOptions>>().Value,
            sp.GetRequiredService<IOptions<DistributedOptions>>().Value,
            sp.GetRequiredService<ILogger<RedisMasterDiscovery>>()));

        return builder;
    }
}
