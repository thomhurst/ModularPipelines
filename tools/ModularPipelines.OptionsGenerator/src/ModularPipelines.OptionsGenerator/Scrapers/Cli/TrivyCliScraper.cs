using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Trivy security scanner.
/// Trivy uses Cobra-style help format (Go-based CLI).
///
/// trivy help format (trivy --help):
/// Scanner for vulnerabilities in container images, file systems, and Git repositories, as well as for configuration issues and hard-coded secrets
///
/// Usage:
///   trivy [global flags] command [flags] target
///   trivy [command]
///
/// Scanning Commands:
///   config      Scan config files for misconfigurations
///   filesystem  Scan local filesystem
///   image       Scan a container image
///   repository  Scan a repository
///   rootfs      Scan rootfs
///   sbom        Scan SBOM for vulnerabilities and licenses
///   vm          [EXPERIMENTAL] Scan a virtual machine image
///
/// Management Commands:
///   module      Manage modules
///   plugin      Manage plugins
///   server      Server mode
///
/// Utility Commands:
///   clean       Remove cached files
///   convert     Convert Trivy JSON report into a different format
///   version     Print the version
/// </summary>
public partial class TrivyCliScraper : CobraCliScraper
{
    public TrivyCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<TrivyCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "trivy";

    public override string NamespacePrefix => "Trivy";

    public override string TargetNamespace => "ModularPipelines.Trivy";

    public override string OutputDirectory => "src/ModularPipelines.Trivy";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "clean"
    };
}
