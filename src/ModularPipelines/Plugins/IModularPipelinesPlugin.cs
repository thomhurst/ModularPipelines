namespace ModularPipelines.Plugins;

/// <summary>
/// Defines a reusable bundle of pipeline configuration, such as services, modules, handlers and options.
/// </summary>
/// <remarks>
/// <para>
/// Add a plugin explicitly with <see cref="PipelineBuilder.AddPlugin{TPlugin}()"/> or
/// <see cref="PipelineBuilder.AddPlugin(IModularPipelinesPlugin)"/>. Plugins are not discovered
/// automatically.
/// </para>
/// <para>
/// <see cref="Configure"/> runs immediately when the plugin is added, so plugins apply in the order they
/// are added, and any configuration made on the builder afterwards (for example by the application)
/// takes precedence over the plugin's.
/// </para>
/// </remarks>
public interface IModularPipelinesPlugin
{
    /// <summary>
    /// Gets the unique name identifying this plugin. A pipeline can add each plugin name once.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Configures the pipeline builder: register services through <see cref="PipelineBuilder.Services"/>,
    /// add modules, event handlers, requirements, or adjust options.
    /// </summary>
    /// <param name="builder">The pipeline builder to configure.</param>
    void Configure(PipelineBuilder builder);
}
