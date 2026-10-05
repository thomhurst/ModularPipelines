using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for yq - YAML/JSON/XML processor (mikefarah/yq).
/// yq uses a Cobra-style help format.
///
/// yq help format (yq --help):
/// yq is a portable command-line YAML, JSON, XML, CSV, TOML and properties processor
///
/// Usage:
///   yq [flags]
///   yq [command]
///
/// Available Commands:
///   eval        Apply expression to files
///   eval-all    Apply expression to all files
///   ...
///
/// Flags:
///   -C, --colors                  force print with colors
///   ...
/// </summary>
public partial class YqCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<YqCliScraper> logger) : CobraCliScraper(executor, helpCache, logger)
{
    public override string ToolName => "yq";

    public override string NamespacePrefix => "Yq";

    public override string TargetNamespace => "ModularPipelines.Yq";

    public override string OutputDirectory => "src/ModularPipelines.Yq";

    /// <inheritdoc />
    protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText) =>
        [.. ParseOptions(helpText, []).Where(option => option.SwitchName is not "--version" and not "--help")];

    /// <inheritdoc />
    protected override IReadOnlyList<CliOptionDefinition> ApplyOptionFixes(
        string[] commandParts,
        IReadOnlyList<CliOptionDefinition> options) =>
        [.. options.Where(option => !GlobalOptions.Any(global => global.SwitchName == option.SwitchName))];

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "shell-completion"
    };

    /// <inheritdoc />
    protected override IReadOnlyList<CliPositionalArgument> ApplyPositionalArgumentFixes(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments) =>
        NormalizeEvalArguments(commandParts, positionalArguments);

    /// <inheritdoc />
    protected override UsageSynopsisParseResult NormalizeUsageSynopsis(
        CliCommandDefinition command,
        UsageSynopsisParseResult usage) =>
        usage with
        {
            PositionalArguments = NormalizeEvalArguments(command.CommandParts, usage.PositionalArguments),
        };

    private static IReadOnlyList<CliPositionalArgument> NormalizeEvalArguments(
        IReadOnlyList<string> commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments) =>
        commandParts is ["eval" or "eval-all"]
            ? [.. positionalArguments
                .Select(argument => argument with
                {
                    Phase = CommandLinePhase.Passthrough,
                    PrependOptionTerminatorIfValueStartsWithDash = true,
                })]
            : positionalArguments;
}
