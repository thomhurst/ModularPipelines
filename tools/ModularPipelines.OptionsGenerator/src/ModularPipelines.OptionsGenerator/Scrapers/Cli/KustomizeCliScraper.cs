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
public partial class KustomizeCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<KustomizeCliScraper> logger) : CobraCliScraper(executor, helpCache, logger)
{
    public override string ToolName => "kustomize";

    public override string NamespacePrefix => "Kustomize";

    public override string TargetNamespace => "ModularPipelines.Kubernetes";

    public override string OutputDirectory => "src/ModularPipelines.Kubernetes";

    protected override string VersionArguments => "version";

    // Root PersistentFlags includes Go's stack-trace flag; build/plugin settings stay local.
    protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText) =>
        [.. ParseNamedOptionSection(helpText, "Flags", []).Where(option => option.SwitchName == "--stack-trace")];

    /// <summary>
    /// Kustomize renders the build directory as required in its Cobra usage line even
    /// though omitting it is documented to use the current directory.
    /// </summary>
    protected override IReadOnlyList<CliPositionalArgument> ApplyPositionalArgumentFixes(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        if (commandParts is not ["build"] || positionalArguments is not [var directory])
        {
            return positionalArguments;
        }

        return
        [
            directory with
            {
                CSharpType = $"{directory.CSharpType.TrimEnd('?')}?",
                IsRequired = false,
            },
        ];
    }

    /// <summary>
    /// Kustomize 5.8 registers create annotations and labels as scalar strings, then
    /// parses one comma-separated key:value value into a map.
    /// </summary>
    protected override IReadOnlyList<CliOptionDefinition> ApplyOptionFixes(
        string[] commandParts,
        IReadOnlyList<CliOptionDefinition> options)
    {
        var globals = EffectiveGlobalOptions;
        var globalSwitches = globals.Select(option => option.SwitchName).ToHashSet(StringComparer.Ordinal);
        CliGlobalOptionMerger.Merge(globals, options.Where(option => globalSwitches.Contains(option.SwitchName)));
        options = [.. options.Where(option => !globalSwitches.Contains(option.SwitchName))];

        if (commandParts is not ["create"])
        {
            return options;
        }

        return [.. options
            .Select(option => option is
            {
                SwitchName: "--annotations" or "--labels",
                CSharpType: "string?",
            }
                    ? option with
                    {
                        CSharpType = "IEnumerable<string>?",
                        CollectionSeparator = ",",
                        IsKeyValue = false,
                    }
                    : option)];
    }

    /// <summary>
    /// Kustomize 5.8 edit commands validate operands in RunE but omit them from Cobra's
    /// Use value, so their usage line reads only "[flags]". Operands follow each
    /// command's argument validation: a trailing "..." marks commands that accept
    /// several space-separated operands.
    /// </summary>
    private static readonly Dictionary<string, string> EditOperandSynopses = new(StringComparer.Ordinal)
    {
        ["add annotation"] = "<annotation>...",
        ["add base"] = "<base>",
        ["add buildmetadata"] = "<metadata>",
        ["add component"] = "<file>...",
        ["add configuration"] = "<file>...",
        ["add generator"] = "<file>...",
        ["add label"] = "<label>...",
        ["add resource"] = "<file>...",
        ["add transformer"] = "<file>...",
        ["remove annotation"] = "<keys>",
        ["remove buildmetadata"] = "<metadata>",
        ["remove component"] = "<file>...",
        ["remove label"] = "<keys>",
        ["remove resource"] = "<file>...",
        ["remove transformer"] = "<file>...",
        ["set annotation"] = "<annotation>...",
        ["set buildmetadata"] = "<metadata>",
        ["set image"] = "<image>...",
        ["set label"] = "<label>...",
        ["set nameprefix"] = "<prefix>",
        ["set namespace"] = "<namespace>",
        ["set namesuffix"] = "<suffix>",
        ["set replicas"] = "<replicas>...",
    };

    protected override IEnumerable<string> GetAdditionalUsageSynopses(
        string[] commandPath,
        string helpText) =>
        commandPath is [_, "edit", var verb, var noun]
        && EditOperandSynopses.TryGetValue($"{verb} {noun}", out var operands)
            ? [$"{string.Join(" ", commandPath)} {operands}"]
            : base.GetAdditionalUsageSynopses(commandPath, helpText);

    protected override void ValidateChildCommandPath(string[] commandPath)
    {
        var repeatedSegment = commandPath
            .Skip(1)
            .GroupBy(segment => segment, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(group => group.Count() > 1);
        if (repeatedSegment is null)
        {
            return;
        }

        throw new InvalidOperationException(
            $"Repeated command-path segment '{repeatedSegment.Key}' discovered in "
            + $"{string.Join(' ', commandPath)}.");
    }

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "docs"
    };
}
