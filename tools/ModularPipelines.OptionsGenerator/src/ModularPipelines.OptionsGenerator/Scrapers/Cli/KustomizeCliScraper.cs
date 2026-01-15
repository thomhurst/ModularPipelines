using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Kustomize - Kubernetes native configuration management.
/// Kustomize uses a Cobra-style help format.
///
/// kustomize help format (kustomize --help):
/// Manages declarative configuration of Kubernetes.
///
/// Usage:
///   kustomize [command]
///
/// Available Commands:
///   build       Print configuration per contents of kustomization.yaml
///   cfg         Commands for reading and writing configuration
///   completion  Generate shell completion script
///   create      Create a new kustomization in the current directory
///   edit        Edits a kustomization file
///   fn          Commands for running functions against configuration
///   version     Prints the kustomize version
///
/// Flags:
///   -h, --help      help for kustomize
///       --stack-trace   print a stack-trace on error
///
/// Subcommand help (kustomize build --help):
/// Build a kustomization target from a directory or URL.
///
/// Usage:
///   kustomize build [path] [flags]
///
/// Flags:
///       --as-current-user    use the uid and gid of the command executor
///       --enable-alpha-plugins  enable alpha plugins
///       ...
/// </summary>
public partial class KustomizeCliScraper : CobraCliScraper
{
    public KustomizeCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<KustomizeCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "kustomize";

    public override string NamespacePrefix => "Kustomize";

    public override string TargetNamespace => "ModularPipelines.Kubernetes";

    public override string OutputDirectory => "src/ModularPipelines.Kubernetes";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "docs"
    };
}
