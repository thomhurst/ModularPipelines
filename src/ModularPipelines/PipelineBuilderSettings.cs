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
    /// Gets the default working directory for commands and context-based relative file paths.
    /// </summary>
    /// <remarks>
    /// When omitted, the configured content root is used when available, then a detected
    /// pipeline project, and finally the process working directory.
    /// Pipeline.CreateBuilder detects a project using MODULAR_PIPELINES_DIRECTORY when set;
    /// otherwise it searches ancestors of the calling source file and then AppContext.BaseDirectory.
    /// Detection requires both appsettings.json and a *.csproj file in the same directory.
    /// An invalid MODULAR_PIPELINES_DIRECTORY value throws rather than falling back.
    /// This does not change the process's current directory. Relative paths passed directly to
    /// FilePath or FolderPath constructors and string conversions remain process-relative;
    /// use context.Files.GetFile or context.Files.GetFolder for pipeline-relative paths.
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
