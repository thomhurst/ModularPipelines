using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
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
public partial class BuildahCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<BuildahCliScraper> logger) : CobraCliScraper(executor, helpCache, logger)
{
    public override string ToolName => "buildah";

    public override string NamespacePrefix => "Buildah";

    public override string TargetNamespace => "ModularPipelines.Buildah";

    public override string OutputDirectory => "src/ModularPipelines.Buildah";

    // Cobra parses the selected command without traversing parent flags. Local
    // UID/GID maps therefore replace the persistent defaults in the same scope.
    protected override bool GlobalOptionsBeforeSubcommands => false;

    protected override bool TreatParseErrorsAsFatal => true;

    protected override string NormalizeOptionTypeHint(
        string[] commandParts, string switchName, string typeHint, string description) =>
        // Buildah names this integer value `number` in its usage description.
        switchName == "--healthcheck-retries" && typeHint.Equals("number", StringComparison.OrdinalIgnoreCase)
            ? "int"
            : typeHint;

    protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText) =>
        [.. ParseNamedOptionSection(helpText, "Flags", [])
            .Where(option => option.SwitchName is not ("--help" or "--version"))
            .Select(NormalizeMappingOption)];

    protected override IReadOnlyList<CliOptionDefinition> ApplyOptionFixes(
        string[] commandParts, IReadOnlyList<CliOptionDefinition> options) =>
        [.. options.Select(NormalizeMappingOption)];

    protected override async Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandParts, string helpText, UsageSynopsisParseResult synopsis, CancellationToken cancellationToken)
    {
        var command = await base.ParseCommandAsync(commandParts, helpText, synopsis, cancellationToken).ConfigureAwait(false);
        if (command is null)
        {
            return null;
        }

        // Current Buildah templates omit inherited help rows. Validate and remove
        // them only when an explicit Global Flags section exposes them in future.
        var globals = EffectiveGlobalOptions;
        // Inherited enum names must match the root scope while their values still
        // come from the command's help, so conflicting definitions remain fatal.
        var inherited = ParseNamedOptionSection(helpText, "Global Flags", [])
            .Where(option => globals.Any(global => global.SwitchName == option.SwitchName))
            .Select(NormalizeMappingOption).ToArray();
        CliGlobalOptionMerger.Merge(globals, inherited);
        if (inherited.Length == 0)
        {
            return command;
        }

        var localSwitches = ParseNamedOptionSection(helpText, "Flags", commandParts)
            .Select(option => option.SwitchName).ToHashSet(StringComparer.Ordinal);
        var inheritedSwitches = inherited.Select(option => option.SwitchName).ToHashSet(StringComparer.Ordinal);
        var options = command.Options
            .Where(option => localSwitches.Contains(option.SwitchName) || !inheritedSwitches.Contains(option.SwitchName))
            .ToArray();
        return command with
        {
            Options = options,
            Enums = [.. options.Where(option => option.EnumDefinition is not null).Select(option => option.EnumDefinition!)],
        };
    }

    private static CliOptionDefinition NormalizeMappingOption(CliOptionDefinition option) =>
        option.SwitchName is "--storage-opt" or "--userns-uid-map" or "--userns-gid-map"
            ? option with { CSharpType = "IEnumerable<string>?", IsCollection = true, AcceptsMultipleValues = true, GroupValues = false, CollectionSeparator = null }
            : option;

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "info"
    };
}
