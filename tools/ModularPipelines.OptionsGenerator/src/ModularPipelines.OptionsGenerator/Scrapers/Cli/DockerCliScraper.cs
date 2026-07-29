using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
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

    public DockerCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<DockerCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    /// <inheritdoc />
    protected override CliCommandGroupAlias? DetectCommandGroupAlias(
        string[] commandPath,
        string helpText) =>
        DockerCliCompatibility.DetectCommandGroupAlias(commandPath, helpText);
}
