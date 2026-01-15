using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Podman - daemonless container engine.
/// Podman uses Cobra for its CLI and is compatible with Docker CLI.
///
/// podman help format (podman --help):
/// Manage pods, containers and images
///
/// Usage:
///   podman [options] [command]
///
/// Available Commands:
///   attach      Attach to a running container
///   build       Build an image using instructions from Containerfiles
///   commit      Create new image based on the changed container
///   ...
///
/// Flags:
///   -h, --help   Help for podman
/// </summary>
public partial class PodmanCliScraper : CobraCliScraper
{
    public PodmanCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<PodmanCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "podman";

    public override string NamespacePrefix => "Podman";

    public override string TargetNamespace => "ModularPipelines.Podman";

    public override string OutputDirectory => "src/ModularPipelines.Podman";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "info"
    };
}
