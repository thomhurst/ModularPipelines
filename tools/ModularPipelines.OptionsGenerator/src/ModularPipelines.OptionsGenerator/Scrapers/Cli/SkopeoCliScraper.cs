using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
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
public partial class SkopeoCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<SkopeoCliScraper> logger) : CobraCliScraper(executor, helpCache, logger)
{
    public override string ToolName => "skopeo";

    public override string NamespacePrefix => "Skopeo";

    public override string TargetNamespace => "ModularPipelines.Skopeo";

    public override string OutputDirectory => "src/ModularPipelines.Skopeo";

    // Public root settings are PersistentFlags. Help/version and the hidden,
    // deprecated root --tls-verify flag are not inherited settings.
    protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText) =>
        [.. ParseNamedOptionSection(helpText, "Flags", [])
            .Where(option => option.SwitchName is not ("--help" or "--version" or "--tls-verify"))
            .Select(NormalizeBooleanOption)];

    protected override bool TreatParseErrorsAsFatal => true;

    protected override bool IsSecretOption(string propertyName, bool isFlag, string description) =>
        propertyName is "Creds" or "SrcCreds" or "DestCreds"
        || base.IsSecretOption(propertyName, isFlag, description);

    protected override IReadOnlyList<CliOptionDefinition> ApplyOptionFixes(
        string[] commandParts, IReadOnlyList<CliOptionDefinition> options)
    {
        var globals = EffectiveGlobalOptions;
        var globalSwitches = globals.Select(option => option.SwitchName).ToHashSet(StringComparer.Ordinal);
        var normalized = options.Select(option => globalSwitches.Contains(option.SwitchName)
            || option.SwitchName is "--tls-verify" or "--src-tls-verify" or "--dest-tls-verify"
                ? NormalizeBooleanOption(option) : option).ToArray();
        CliGlobalOptionMerger.Merge(globals, normalized.Where(option => globalSwitches.Contains(option.SwitchName)));
        return [.. normalized.Where(option => !globalSwitches.Contains(option.SwitchName))];
    }

    private static CliOptionDefinition NormalizeBooleanOption(CliOptionDefinition option) =>
        option.CSharpType == "bool?"
            ? option with { IsFlag = false, ValueSeparator = "=", ValueArity = CliOptionValueArity.Optional }
            : option;

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version"
    };
}
