using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Distributed.Configuration;
using ModularPipelines.Plugins;

namespace ModularPipelines.Distributed.Extensions;

/// <summary>
/// Extension methods for configuring distributed pipeline mode.
/// </summary>
public static class DistributedPipelineBuilderExtensions
{
    private static int _pluginRegistered;

    /// <summary>
    /// Enables distributed execution mode and registers the distributed plugin.
    /// </summary>
    /// <returns></returns>
    public static PipelineBuilder AddDistributedMode(this PipelineBuilder builder, Action<DistributedOptions> configure)
    {
        var options = new DistributedOptions();
        configure(options);
        options.Enabled = true;
        builder.Services.AddSingleton(Microsoft.Extensions.Options.Options.Create(options));

        EnsurePluginRegistered();

        return builder;
    }

    /// <summary>
    /// Enables distributed execution mode from configuration.
    /// </summary>
    /// <returns></returns>
    public static PipelineBuilder AddDistributedMode(this PipelineBuilder builder, IConfigurationSection section)
    {
        builder.Services.Configure<DistributedOptions>(section);

        // Also ensure Enabled is set
        builder.Services.PostConfigure<DistributedOptions>(o => o.Enabled = true);
        EnsurePluginRegistered();
        return builder;
    }

    /// <summary>
    /// Registers a custom distributed coordinator implementation.
    /// </summary>
    /// <returns></returns>
    public static PipelineBuilder AddDistributedCoordinator<TCoordinator>(this PipelineBuilder builder)
        where TCoordinator : class, IDistributedCoordinator
    {
        builder.Services.AddSingleton<IDistributedCoordinator, TCoordinator>();
        return builder;
    }

    /// <summary>
    /// Registers a distributed coordinator factory for async initialization.
    /// </summary>
    /// <returns></returns>
    public static PipelineBuilder AddDistributedCoordinatorFactory<TFactory>(this PipelineBuilder builder)
        where TFactory : class, IDistributedCoordinatorFactory
    {
        builder.Services.AddSingleton<IDistributedCoordinatorFactory, TFactory>();
        return builder;
    }

    private static void EnsurePluginRegistered()
    {
        if (Interlocked.CompareExchange(ref _pluginRegistered, 1, 0) == 0)
        {
            PluginRegistry.Register(new DistributedPipelinePlugin());
        }
    }
}
