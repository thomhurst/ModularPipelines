namespace ModularPipelines;

/// <summary>
/// Settings for creating a pipeline builder.
/// </summary>
public sealed record PipelineBuilderSettings
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
    /// When omitted, the configured content root is used when available, then the calling
    /// source file's project directory, and finally the process working directory.
    /// </remarks>
    public string? WorkingDirectory { get; init; }

    /// <summary>
    /// Gets a value indicating whether assemblies whose filenames contain
    /// <c>ModularPipelines</c> are eagerly loaded from the application directory.
    /// </summary>
    /// <remarks>
    /// Disabled by default. Enable this only when a plugin relies on module initializers
    /// instead of explicit assembly or service registration.
    /// </remarks>
    public bool LoadModularPipelinesAssemblies { get; init; }
}
