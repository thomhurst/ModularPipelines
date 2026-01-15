using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Skopeo - container image operations tool.
/// Skopeo uses Cobra for its CLI.
///
/// skopeo help format (skopeo --help):
/// Various operations with container images and container image registries
///
/// Usage:
///   skopeo [command]
///
/// Available Commands:
///   copy        Copy an IMAGE-NAME from one location to another
///   delete      Delete image IMAGE-NAME
///   inspect     Inspect image IMAGE-NAME
///   list-tags   List tags in the repository
///   ...
/// </summary>
public partial class SkopeoCliScraper : CobraCliScraper
{
    public SkopeoCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<SkopeoCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "skopeo";

    public override string NamespacePrefix => "Skopeo";

    public override string TargetNamespace => "ModularPipelines.Skopeo";

    public override string OutputDirectory => "src/ModularPipelines.Skopeo";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version"
    };
}
