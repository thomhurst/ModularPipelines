using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Distributed.SignalR.Coordination;

namespace ModularPipelines.Distributed.SignalR;

/// <summary>
/// Extension methods for registering the SignalR distributed coordinator.
/// </summary>
public static class SignalRDistributedExtensions
{
    /// <summary>
    /// Registers the SignalR-based distributed coordinator factory.
    /// Must be called after <c>AddDistributedMode</c>.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configure">Optional configuration action for SignalR options.</param>
    /// <returns>The pipeline builder for chaining.</returns>
    public static PipelineBuilder AddSignalRDistributedCoordinator(
        this PipelineBuilder builder,
        Action<SignalRDistributedOptions>? configure = null)
    {
        if (configure is not null)
        {
            builder.Services.Configure(configure);
        }

        builder.Services.AddSingleton<IDistributedCoordinatorFactory, SignalRDistributedCoordinatorFactory>();

        return builder;
    }

    /// <summary>
    /// Registers the SignalR-based distributed coordinator factory from configuration.
    /// Must be called after <c>AddDistributedMode</c>.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="section">The configuration section containing SignalR options.</param>
    /// <returns>The pipeline builder for chaining.</returns>
    [RequiresUnreferencedCode("Configuration binding requires members of SignalRDistributedOptions that cannot be statically discovered.")]
    [RequiresDynamicCode("Configuration binding may require runtime code generation.")]
    public static PipelineBuilder AddSignalRDistributedCoordinator(
        this PipelineBuilder builder,
        IConfigurationSection section)
    {
        builder.Services.Configure<SignalRDistributedOptions>(section);
        builder.Services.AddSingleton<IDistributedCoordinatorFactory, SignalRDistributedCoordinatorFactory>();

        return builder;
    }
}
