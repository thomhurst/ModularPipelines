using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Grype - vulnerability scanner from Anchore.
/// Grype uses Cobra for its CLI (Go-based).
///
/// grype help format (grype --help):
/// A vulnerability scanner for container images, filesystems, and SBOMs.
///
/// Usage:
///   grype [IMAGE] [flags]
///   grype [command]
///
/// Available Commands:
///   completion  Generate a shell completion script
///   config      Show the current configuration
///   db          vulnerability database operations
///   help        Help about any command
///   version     Show application version
///
/// Grype is a vulnerability scanner for container images and filesystems.
/// It works with Syft to generate SBOMs and scan them for known vulnerabilities.
/// Supports scanning container images, directories, archives, and SBOMs.
/// </summary>
public partial class GrypeCliScraper : CobraCliScraper
{
    public GrypeCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<GrypeCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "grype";

    public override string NamespacePrefix => "Grype";

    public override string TargetNamespace => "ModularPipelines.Grype";

    public override string OutputDirectory => "src/ModularPipelines.Grype";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "config"
    };
}
