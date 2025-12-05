using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Helm.
/// Helm is a Cobra-based CLI with consistent help formatting.
/// </summary>
public class HelmCliScraper : CobraCliScraper
{
    public override string ToolName => "helm";
    public override string NamespacePrefix => "Helm";
    public override string TargetNamespace => "ModularPipelines.Helm";
    public override string OutputDirectory => "src/ModularPipelines.Helm";

    public HelmCliScraper(ICliCommandExecutor executor, ILogger<HelmCliScraper> logger)
        : base(executor, logger)
    {
    }
}
