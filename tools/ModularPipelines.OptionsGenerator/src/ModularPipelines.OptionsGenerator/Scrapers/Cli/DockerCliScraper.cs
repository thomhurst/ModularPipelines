using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Docker.
/// Docker is a Cobra-based CLI with consistent help formatting.
/// </summary>
public class DockerCliScraper : CobraCliScraper
{
    public override string ToolName => "docker";
    public override string NamespacePrefix => "Docker";
    public override string TargetNamespace => "ModularPipelines.Docker";
    public override string OutputDirectory => "src/ModularPipelines.Docker";

    public DockerCliScraper(ICliCommandExecutor executor, ILogger<DockerCliScraper> logger)
        : base(executor, logger)
    {
    }

    /// <summary>
    /// Docker has some additional skip patterns.
    /// </summary>
    protected override bool IsSkippableSubcommand(string subcommand)
    {
        if (base.IsSkippableSubcommand(subcommand))
        {
            return true;
        }

        // Docker has some management commands that just group others
        var lowerName = subcommand.ToLowerInvariant();
        return lowerName is "app" or "buildx" or "compose" or "context" or "manifest" or
               "plugin" or "sbom" or "scan" or "scout" or "stack" or "swarm" or "trust";
    }
}
