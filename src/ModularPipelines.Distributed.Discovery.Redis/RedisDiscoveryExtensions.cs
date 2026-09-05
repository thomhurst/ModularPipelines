using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Extension methods for registering Redis-based master endpoint discovery.
/// </summary>
public static class RedisDiscoveryExtensions
{
    /// <summary>
    /// Registers Redis-based master endpoint discovery.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configure">Configuration action for Redis discovery options.</param>
    /// <returns>The pipeline builder for chaining.</returns>
    public static PipelineBuilder AddRedisMasterDiscovery(
        this PipelineBuilder builder,
        Action<RedisDiscoveryOptions> configure)
    {
        builder.Services.Configure(configure);
        return AddRedisMasterDiscoveryServices(builder);
    }

    /// <summary>
    /// Registers Redis-based master endpoint discovery from configuration.
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
        builder.Services.Configure<RedisDiscoveryOptions>(section);
        return AddRedisMasterDiscoveryServices(builder);
    }

    private static PipelineBuilder AddRedisMasterDiscoveryServices(PipelineBuilder builder)
    {
        builder.Services.Configure<DistributedOptions>(options => options.RequireExplicitRunId = true);
        builder.Services.AddOptions<DistributedOptions>().ValidateOnStart();
        builder.Services.AddOptions<RedisDiscoveryOptions>()
            .Validate(
                options => string.IsNullOrWhiteSpace(options.RestUrl)
                           == string.IsNullOrWhiteSpace(options.RestToken),
                "RestUrl and RestToken must be configured together.")
            .ValidateOnStart();

        builder.Services.TryAddSingleton<IConnectionMultiplexer>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<RedisDiscoveryOptions>>().Value;
            return ConnectionMultiplexer.Connect(opts.ConnectionString);
        });
        builder.Services.AddSingleton<IRedisDiscoveryStore>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<RedisDiscoveryOptions>>().Value;
            return !string.IsNullOrWhiteSpace(opts.RestUrl)
                ? new RestRedisDiscoveryStore(opts.RestUrl!, opts.RestToken!)
                : new StackExchangeRedisDiscoveryStore(sp.GetRequiredService<IConnectionMultiplexer>());
        });
        builder.Services.AddSingleton<IMasterDiscovery>(sp => new RedisMasterDiscovery(
            sp.GetRequiredService<IRedisDiscoveryStore>(),
            sp.GetRequiredService<IOptions<RedisDiscoveryOptions>>().Value,
            sp.GetRequiredService<IOptions<DistributedOptions>>().Value,
            sp.GetRequiredService<ILogger<RedisMasterDiscovery>>()));

        return builder;
    }
}
