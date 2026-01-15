using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.Plugins;

/// <summary>
/// Defines a plugin that can extend ModularPipelines functionality.
/// Plugins self-register via [ModuleInitializer] methods calling <see cref="PluginRegistry.Register"/>.
/// </summary>
public interface IModularPipelinesPlugin
{
    /// <summary>
    /// Gets the unique name identifying this plugin.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the execution priority. Lower values run first. Default is 0.
    /// </summary>
    int Priority => 0;

    /// <summary>
    /// Configure services in the DI container.
    /// Called during host setup, before the pipeline is built.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    void ConfigureServices(IServiceCollection services);

    /// <summary>
    /// Configure the pipeline builder.
    /// Register hooks, modules, and other pipeline components.
    /// Called after services are configured, before execution.
    /// </summary>
    /// <param name="pipelineBuilder">The pipeline builder to configure.</param>
    void ConfigurePipeline(PipelineBuilder pipelineBuilder);
}
