using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.SignalR.Coordination;

namespace ModularPipelines.Distributed.SignalR;

/// <summary>
/// Extension methods for registering the SignalR distributed coordinator.
/// </summary>
public static class SignalRDistributedExtensions
{
    /// <summary>
    /// Registers the SignalR-based distributed coordinator. The master hosts a SignalR hub and
    /// workers connect to it. Options are validated at startup.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configure">Configures the SignalR options.</param>
    /// <returns>The pipeline builder for chaining.</returns>
    public static PipelineBuilder AddSignalRDistributedCoordinator(
        this PipelineBuilder builder,
        Action<SignalRDistributedOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        builder.Services.Configure(configure);
        return AddSignalRDistributedCoordinatorServices(builder);
    }

    /// <summary>
    /// Registers the SignalR-based distributed coordinator from configuration. Options are
    /// validated at startup.
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
        return AddSignalRDistributedCoordinatorServices(builder);
    }

    private static PipelineBuilder AddSignalRDistributedCoordinatorServices(PipelineBuilder builder)
    {
        // Workers check that they join the master's run, so every process needs the same identifier.
        builder.RequireExplicitRunId();
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IValidateOptions<SignalRDistributedOptions>, SignalRDistributedOptionsValidator>());
        builder.Services.AddOptions<SignalRDistributedOptions>().ValidateOnStart();
        return builder.AddDistributedCoordinatorFactory<SignalRDistributedCoordinatorFactory>();
    }
}
