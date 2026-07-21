using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.SignalR.Discovery;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Extension methods for registering Redis-based master URL discovery.
/// </summary>
public static class RedisDiscoveryExtensions
{
    /// <summary>
    /// Registers Redis-based master URL discovery for the SignalR distributed coordinator.
    /// Must be called after <c>AddSignalRDistributedCoordinator</c>.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configure">Configuration action for Redis discovery options.</param>
    /// <returns>The pipeline builder for chaining.</returns>
    public static PipelineBuilder AddRedisSignalRDiscovery(
        this PipelineBuilder builder,
        Action<RedisDiscoveryOptions> configure)
    {
        var options = new RedisDiscoveryOptions();
        configure(options);

        var hasRestUrl = !string.IsNullOrWhiteSpace(options.RestUrl);
        var hasRestToken = !string.IsNullOrWhiteSpace(options.RestToken);
        if (hasRestUrl != hasRestToken)
        {
            throw new ArgumentException("RestUrl and RestToken must be configured together.", nameof(configure));
        }

        builder.Services.AddSingleton(options);
        builder.Services.TryAddSingleton<IConnectionMultiplexer>(sp =>
        {
            var opts = sp.GetRequiredService<RedisDiscoveryOptions>();
            return ConnectionMultiplexer.Connect(opts.ConnectionString);
        });
        builder.Services.AddSingleton<IRedisDiscoveryStore>(sp =>
        {
            var opts = sp.GetRequiredService<RedisDiscoveryOptions>();
            return hasRestUrl
                ? new RestRedisDiscoveryStore(opts.RestUrl!, opts.RestToken!)
                : new StackExchangeRedisDiscoveryStore(sp.GetRequiredService<IConnectionMultiplexer>());
        });
        builder.Services.AddSingleton<ISignalRMasterDiscovery>(sp => new RedisSignalRMasterDiscovery(
            sp.GetRequiredService<IRedisDiscoveryStore>(),
            sp.GetRequiredService<RedisDiscoveryOptions>(),
            sp.GetRequiredService<ILogger<RedisSignalRMasterDiscovery>>()));

        return builder;
    }
}
