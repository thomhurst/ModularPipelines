namespace ModularPipelines;

/// <summary>
/// Options for configuring the pipeline builder.
/// </summary>
public class PipelineBuilderOptions
{
    /// <summary>
    /// Gets or sets the command line arguments.
    /// </summary>
    public string[]? Args { get; set; }

    /// <summary>
    /// Gets or sets the application name.
    /// </summary>
    public string? ApplicationName { get; set; }

    /// <summary>
    /// Gets or sets the environment name.
    /// </summary>
    public string? EnvironmentName { get; set; }

    /// <summary>
    /// Gets or sets the content root path.
    /// </summary>
    public string? ContentRootPath { get; set; }
}
