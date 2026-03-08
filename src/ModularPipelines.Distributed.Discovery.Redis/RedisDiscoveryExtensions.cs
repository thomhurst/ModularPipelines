using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

        builder.Services.AddSingleton(options);
        builder.Services.TryAddSingleton<IConnectionMultiplexer>(sp =>
        {
            var opts = sp.GetRequiredService<RedisDiscoveryOptions>();
            return ConnectionMultiplexer.Connect(opts.ConnectionString);
        });
        builder.Services.AddSingleton<ISignalRMasterDiscovery, RedisSignalRMasterDiscovery>();

        return builder;
    }
}
