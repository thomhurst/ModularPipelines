namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// Represents a complete CLI tool definition with all its commands.
/// </summary>
public record CliToolDefinition
{
    /// <summary>
    /// The tool name (e.g., "kubectl", "docker", "az").
    /// </summary>
    public required string ToolName { get; init; }

    /// <summary>
    /// The namespace prefix for generated classes (e.g., "Kubernetes", "Docker", "Azure").
    /// </summary>
    public required string NamespacePrefix { get; init; }

    /// <summary>
    /// The target namespace for generated options (e.g., "ModularPipelines.Kubernetes").
    /// </summary>
    public required string TargetNamespace { get; init; }

    /// <summary>
    /// The output directory relative to the repository root.
    /// </summary>
    public required string OutputDirectory { get; init; }

    /// <summary>
    /// All commands for this tool.
    /// </summary>
    public required IReadOnlyList<CliCommandDefinition> Commands { get; init; }

    /// <summary>
    /// Global options that apply to all commands.
    /// </summary>
    public IReadOnlyList<CliOptionDefinition> GlobalOptions { get; init; } = [];

    /// <summary>
    /// Sub-domain groups for organizing commands (e.g., "Container", "Image" for Docker).
    /// </summary>
    public IReadOnlyList<string> SubDomainGroups => Commands
        .Where(c => c.SubDomainGroup is not null)
        .Select(c => c.SubDomainGroup!)
        .Distinct()
        .OrderBy(g => g)
        .ToList();

    /// <summary>
    /// All enums that need to be generated for this tool.
    /// </summary>
    public IReadOnlyList<CliEnumDefinition> AllEnums => Commands
        .SelectMany(c => c.Enums)
        .DistinctBy(e => e.EnumName)
        .ToList();

    /// <summary>
    /// Any scraping errors encountered.
    /// </summary>
    public IReadOnlyList<ScrapingError> Errors { get; init; } = [];
}

/// <summary>
/// Represents an error that occurred during scraping.
/// </summary>
public record ScrapingError
{
    /// <summary>
    /// The URL or command that failed.
    /// </summary>
    public required string Source { get; init; }

    /// <summary>
    /// The error message.
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// The exception if available.
    /// </summary>
    public Exception? Exception { get; init; }
}
