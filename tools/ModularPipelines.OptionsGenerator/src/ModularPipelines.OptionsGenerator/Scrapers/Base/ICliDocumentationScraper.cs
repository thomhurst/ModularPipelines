using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers.Base;

/// <summary>
/// Defines the contract for CLI documentation scrapers.
/// </summary>
public interface ICliDocumentationScraper
{
    /// <summary>
    /// The tool name (e.g., "kubectl", "docker", "az").
    /// </summary>
    string ToolName { get; }

    /// <summary>
    /// The namespace prefix for generated classes (e.g., "Kubernetes", "Docker").
    /// </summary>
    string NamespacePrefix { get; }

    /// <summary>
    /// The target namespace for generated options.
    /// </summary>
    string TargetNamespace { get; }

    /// <summary>
    /// The output directory relative to the repository root.
    /// </summary>
    string OutputDirectory { get; }

    /// <summary>
    /// Scrapes the CLI documentation and returns a complete tool definition.
    /// </summary>
    Task<CliToolDefinition> ScrapeAsync(CancellationToken cancellationToken = default);
}
