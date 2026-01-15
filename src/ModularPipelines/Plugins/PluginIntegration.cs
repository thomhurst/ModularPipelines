using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Exceptions;

namespace ModularPipelines.Plugins;

/// <summary>
/// Handles integration of registered plugins into the pipeline setup process.
/// </summary>
internal static class PluginIntegration
{
    /// <summary>
    /// Applies all registered plugins' service configuration.
    /// Called during DI setup, before the host is built.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <exception cref="PluginInitializationException">Thrown if a plugin fails to configure services.</exception>
    public static void ApplyPluginServices(IServiceCollection services)
    {
        foreach (var plugin in PluginRegistry.Plugins)
        {
            try
            {
                plugin.ConfigureServices(services);
            }
            catch (Exception ex)
            {
                throw new PluginInitializationException(
                    $"Plugin '{plugin.Name}' failed during ConfigureServices: {ex.Message}",
                    plugin.Name,
                    ex);
            }
        }
    }

    /// <summary>
    /// Applies all registered plugins' pipeline configuration.
    /// Called during pipeline building, after services are configured.
    /// </summary>
    /// <param name="pipelineBuilder">The pipeline builder to configure.</param>
    /// <exception cref="PluginInitializationException">Thrown if a plugin fails to configure the pipeline.</exception>
    public static void ApplyPluginConfiguration(PipelineBuilder pipelineBuilder)
    {
        foreach (var plugin in PluginRegistry.Plugins)
        {
            try
            {
                plugin.ConfigurePipeline(pipelineBuilder);
            }
            catch (Exception ex)
            {
                throw new PluginInitializationException(
                    $"Plugin '{plugin.Name}' failed during ConfigurePipeline: {ex.Message}",
                    plugin.Name,
                    ex);
            }
        }
    }
}
