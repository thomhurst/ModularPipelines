using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Pulumi - Infrastructure as Code tool.
/// Pulumi uses Cobra for its CLI.
///
/// pulumi help format (pulumi --help):
/// Pulumi - Modern Infrastructure as Code
///
/// Usage:
///   pulumi [command]
///
/// Available Commands:
///   cancel      Cancel a stack's currently running update
///   config      Manage configuration
///   destroy     Destroy all existing resources
///   ...
///
/// Flags:
///   -h, --help   Help for pulumi
///
/// Subcommand help (pulumi up --help):
/// Deploy resources to a stack...
/// </summary>
public partial class PulumiCliScraper : CobraCliScraper
{
    public PulumiCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<PulumiCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "pulumi";

    public override string NamespacePrefix => "Pulumi";

    public override string TargetNamespace => "ModularPipelines.Pulumi";

    public override string OutputDirectory => "src/ModularPipelines.Pulumi";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "about", "gen-completion", "schema"
    };
}
