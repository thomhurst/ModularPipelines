using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for eksctl - Amazon EKS cluster management.
/// eksctl uses Cobra for its CLI.
///
/// eksctl help format (eksctl --help):
/// The official CLI for Amazon EKS
///
/// Usage: eksctl [command] [flags]
///
/// Commands:
///   associate         Associate resources with a cluster
///   completion        Generates shell completion scripts
///   create            Create resource(s)
///   delete            Delete resource(s)
///   ...
/// </summary>
public partial class EksctlCliScraper : CobraCliScraper
{
    public EksctlCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<EksctlCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "eksctl";

    public override string NamespacePrefix => "Eksctl";

    public override string TargetNamespace => "ModularPipelines.Eksctl";

    public override string OutputDirectory => "src/ModularPipelines.Eksctl";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "info"
    };
}
