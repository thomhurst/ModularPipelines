using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Buildah - OCI container image builder.
/// Buildah uses Cobra for its CLI.
///
/// buildah help format (buildah --help):
/// A command line tool for building OCI container images.
///
/// Usage:
///   buildah [flags]
///   buildah [command]
///
/// Available Commands:
///   add         Add content to the container
///   bud         Build an image using instructions in a Containerfile
///   commit      Create an image from a container
///   ...
/// </summary>
public partial class BuildahCliScraper : CobraCliScraper
{
    public BuildahCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<BuildahCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "buildah";

    public override string NamespacePrefix => "Buildah";

    public override string TargetNamespace => "ModularPipelines.Buildah";

    public override string OutputDirectory => "src/ModularPipelines.Buildah";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "info"
    };
}
