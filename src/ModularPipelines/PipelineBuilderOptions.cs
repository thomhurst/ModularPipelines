namespace ModularPipelines;

/// <summary>
/// Options for configuring the pipeline builder.
/// </summary>
public class PipelineBuilderOptions
{
    /// <summary>
    /// Gets or sets the command line arguments.
    /// </summary>
    public IReadOnlyList<string>? Args { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether ModularPipelines should consume its first-class
    /// command-line options. Disable this to forward every argument directly to host configuration.
    /// </summary>
    public bool EnableCommandLineOptions { get; set; } = true;

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
