using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// Defines the contract for CLI-first scrapers that parse --help output directly.
/// Unlike HTML scrapers, these get authoritative information from the CLI itself.
/// </summary>
public interface ICliScraper
{
    /// <summary>
    /// The CLI tool name (e.g., "helm", "docker", "kubectl").
    /// </summary>
    string ToolName { get; }

    /// <summary>
    /// The namespace prefix for generated classes (e.g., "Helm", "Docker", "Kubernetes").
    /// </summary>
    string NamespacePrefix { get; }

    /// <summary>
    /// The target namespace for generated options (e.g., "ModularPipelines.Helm").
    /// </summary>
    string TargetNamespace { get; }

    /// <summary>
    /// The output directory relative to the repository root.
    /// </summary>
    string OutputDirectory { get; }

    /// <summary>
    /// Whether the CLI tool is available on the system.
    /// </summary>
    Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Scrapes the CLI by parsing --help output and returns a complete tool definition.
    /// </summary>
    Task<CliToolDefinition> ScrapeAsync(CancellationToken cancellationToken = default);
}
