using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Kind (Kubernetes IN Docker).
/// Kind uses Cobra for its CLI (Go-based).
///
/// kind help format (kind --help):
/// kind creates and manages local Kubernetes clusters using Docker container 'nodes'
///
/// Usage:
///   kind [command]
///
/// Available Commands:
///   build       Build one of [node-image]
///   completion  Output shell completion code for the specified shell
///   create      Creates one of [cluster]
///   delete      Deletes one of [cluster]
///   export      Exports one of [kubeconfig, logs]
///   get         Gets one of [clusters, kubeconfig, nodes]
///   help        Help about any command
///   load        Loads images into nodes
///   version     Prints the kind CLI version
/// </summary>
public partial class KindCliScraper : CobraCliScraper
{
    public KindCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<KindCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "kind";

    public override string NamespacePrefix => "Kind";

    public override string TargetNamespace => "ModularPipelines.Kind";

    public override string OutputDirectory => "src/ModularPipelines.Kind";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version"
    };
}
