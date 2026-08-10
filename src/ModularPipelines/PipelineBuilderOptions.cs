namespace ModularPipelines;

/// <summary>
/// Options for configuring the pipeline builder.
/// </summary>
public sealed record PipelineBuilderOptions
{
    /// <summary>
    /// Gets the command line arguments.
    /// </summary>
    public IReadOnlyList<string>? Args { get; init; }

    /// <summary>
    /// Gets a value indicating whether ModularPipelines should consume its first-class
    /// command-line options. Disable this to forward every argument directly to host configuration.
    /// </summary>
    public bool EnableCommandLineOptions { get; init; } = true;

    /// <summary>
    /// Gets the application name.
    /// </summary>
    public string? ApplicationName { get; init; }

    /// <summary>
    /// Gets the environment name.
    /// </summary>
    public string? EnvironmentName { get; init; }

    /// <summary>
    /// Gets the content root path.
    /// </summary>
    public string? ContentRootPath { get; init; }

    /// <summary>
    /// Gets the default working directory for commands and relative file paths.
    /// </summary>
    /// <remarks>
    /// When omitted, the configured content root is used, falling back to the process working directory.
    /// </remarks>
    public string? WorkingDirectory { get; init; }
}
