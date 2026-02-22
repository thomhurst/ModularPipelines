using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.Distributed.Extensions;

/// <summary>
/// Extension methods for configuring distributed pipeline mode.
/// </summary>
public static class DistributedPipelineBuilderExtensions
{
    /// <summary>
    /// Enables distributed execution mode. When <see cref="DistributedOptions.TotalInstances"/> is greater than 1,
    /// the pipeline switches to master/worker mode. Otherwise, execution remains in-process.
    /// </summary>
    public static PipelineBuilder AddDistributedMode(this PipelineBuilder builder, Action<DistributedOptions> configure)
    {
        var options = new DistributedOptions();
        configure(options);
        options.Enabled = true;
        builder.Services.AddSingleton(Microsoft.Extensions.Options.Options.Create(options));

        return builder;
    }

    /// <summary>
    /// Enables distributed execution mode from configuration.
    /// </summary>
    public static PipelineBuilder AddDistributedMode(this PipelineBuilder builder, IConfigurationSection section)
    {
        builder.Services.Configure<DistributedOptions>(section);

        // Also ensure Enabled is set
        builder.Services.PostConfigure<DistributedOptions>(o => o.Enabled = true);
        return builder;
    }

    /// <summary>
    /// Registers a custom distributed coordinator implementation.
    /// </summary>
    public static PipelineBuilder AddDistributedCoordinator<TCoordinator>(this PipelineBuilder builder)
        where TCoordinator : class, IDistributedCoordinator
    {
        builder.Services.AddSingleton<IDistributedCoordinator, TCoordinator>();
        return builder;
    }

    /// <summary>
    /// Registers a distributed coordinator factory for async initialization.
    /// </summary>
    public static PipelineBuilder AddDistributedCoordinatorFactory<TFactory>(this PipelineBuilder builder)
        where TFactory : class, IDistributedCoordinatorFactory
    {
        builder.Services.AddSingleton<IDistributedCoordinatorFactory, TFactory>();
        return builder;
    }
}
