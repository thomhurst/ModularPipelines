using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Distributed.SignalR.Configuration;
using ModularPipelines.Distributed.SignalR.Coordination;

namespace ModularPipelines.Distributed.SignalR.Extensions;

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
        var options = new SignalRDistributedOptions();
        configure?.Invoke(options);

        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton<IDistributedCoordinatorFactory, SignalRDistributedCoordinatorFactory>();

        return builder;
    }
}
