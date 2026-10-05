using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
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
public partial class GrypeCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<GrypeCliScraper> logger)
    : CobraCliScraper(executor, helpCache, logger)
{
    public override string ToolName => "grype";

    public override string NamespacePrefix => "Grype";

    public override string TargetNamespace => "ModularPipelines.Grype";

    public override string OutputDirectory => "src/ModularPipelines.Grype";

    // Grype's clio setup registers these settings on Cobra's persistent flag set.
    // See the Grype 0.120.0 fixture README for version-pinned parser evidence.
    // Other root flags configure scanning only and must not leak into db commands.
    protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText) =>
        [.. ParseNamedOptionSection(helpText, "Flags", []).Where(option =>
            option.SwitchName is "--config" or "--profile" or "--quiet" or "--verbose")];

    protected override IReadOnlyList<CliOptionDefinition> ApplyOptionFixes(
        string[] commandParts,
        IReadOnlyList<CliOptionDefinition> options)
    {
        var globals = EffectiveGlobalOptions;
        var globalSwitches = globals.Select(option => option.SwitchName).ToHashSet(StringComparer.Ordinal);
        CliGlobalOptionMerger.Merge(globals, options.Where(option => globalSwitches.Contains(option.SwitchName)));
        return [.. options.Where(option => !globalSwitches.Contains(option.SwitchName))];
    }

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "config"
    };
}
