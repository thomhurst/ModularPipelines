using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Flux CLI - GitOps toolkit for Kubernetes.
/// Flux uses Cobra for its CLI.
///
/// flux help format (flux --help):
/// Command line utility for assembling Kubernetes CD pipelines
///
/// Usage:
///   flux [command]
///
/// Available Commands:
///   bootstrap     Bootstrap toolkit components
///   build         Build a resource
///   check         Check requirements and installation
///   create        Create or update sources and resources
///   ...
/// </summary>
public partial class FluxCliScraper : CobraCliScraper
{
    public FluxCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<FluxCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "flux";

    public override string NamespacePrefix => "Flux";

    public override string TargetNamespace => "ModularPipelines.Flux";

    public override string OutputDirectory => "src/ModularPipelines.Flux";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version"
    };
}
