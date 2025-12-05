namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// Represents a parsed CLI command with its options and metadata.
/// </summary>
public record CliCommandDefinition
{
    /// <summary>
    /// Full command path (e.g., "kubectl config view", "docker buildx build").
    /// </summary>
    public required string FullCommand { get; init; }

    /// <summary>
    /// Command path parts for CommandPrecedingArguments attribute.
    /// </summary>
    public required string[] CommandParts { get; init; }

    /// <summary>
    /// Generated class name (e.g., "KubernetesConfigViewOptions").
    /// </summary>
    public required string ClassName { get; init; }

    /// <summary>
    /// Parent class name (e.g., "KubernetesOptions").
    /// </summary>
    public required string ParentClassName { get; init; }

    /// <summary>
    /// The tool namespace prefix (e.g., "Kubernetes", "Docker").
    /// </summary>
    public required string ToolNamespacePrefix { get; init; }

    /// <summary>
    /// Description for XML documentation.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Documentation URL for reference.
    /// </summary>
    public string? DocumentationUrl { get; init; }

    /// <summary>
    /// All options/flags for this command.
    /// </summary>
    public required IReadOnlyList<CliOptionDefinition> Options { get; init; }

    /// <summary>
    /// Positional arguments for this command.
    /// </summary>
    public IReadOnlyList<CliPositionalArgument> PositionalArguments { get; init; } = [];

    /// <summary>
    /// Required options that should be constructor parameters.
    /// </summary>
    public IReadOnlyList<CliOptionDefinition> RequiredOptions =>
        Options.Where(o => o.IsRequired).ToList();

    /// <summary>
    /// The sub-domain group this command belongs to (e.g., "Container", "Image" for Docker).
    /// </summary>
    public string? SubDomainGroup { get; init; }

    /// <summary>
    /// Enums that need to be generated for this command's options.
    /// </summary>
    public IReadOnlyList<CliEnumDefinition> Enums { get; init; } = [];
}
