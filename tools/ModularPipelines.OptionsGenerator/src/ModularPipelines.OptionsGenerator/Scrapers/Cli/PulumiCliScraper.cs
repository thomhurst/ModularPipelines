using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Pulumi - Infrastructure as Code tool.
/// Pulumi uses Cobra for its CLI.
///
/// pulumi help format (pulumi --help):
/// Pulumi - Modern Infrastructure as Code
///
/// Usage:
///   pulumi [command]
///
/// Available Commands:
///   cancel      Cancel a stack's currently running update
///   config      Manage configuration
///   destroy     Destroy all existing resources
///   ...
///
/// Flags:
///   -h, --help   Help for pulumi
///
/// Subcommand help (pulumi up --help):
/// Deploy resources to a stack...
/// </summary>
public partial class PulumiCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<PulumiCliScraper> logger)
    : CobraCliScraper(executor, helpCache, logger)
{
    public override string ToolName => "pulumi";

    public override string NamespacePrefix => "Pulumi";

    public override string TargetNamespace => "ModularPipelines.Pulumi";

    public override string OutputDirectory => "src/ModularPipelines.Pulumi";

    // Pulumi registers every public root flag except help/version on PersistentFlags.
    // See Fixtures/Pulumi/3.267.0/README.md for the version-pinned parser evidence.
    protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText) =>
        [.. ParseNamedOptionSection(helpText, "Flags", []).Where(option =>
            option.SwitchName is not ("--help" or "--version"))];

    protected override IReadOnlyList<CliOptionDefinition> ApplyOptionFixes(
        string[] commandParts,
        IReadOnlyList<CliOptionDefinition> options)
    {
        var globals = EffectiveGlobalOptions;
        var globalSwitches = globals.Select(option => option.SwitchName).ToHashSet(StringComparer.Ordinal);
        CliGlobalOptionMerger.Merge(globals, options.Where(option => globalSwitches.Contains(option.SwitchName)));
        return [.. options.Where(option => !globalSwitches.Contains(option.SwitchName))];
    }

    // Emoji defaults to true on macOS and false elsewhere. Always retain explicit false.
    protected override bool IsBooleanValueOption(string[] commandParts, string switchName, string description) =>
        switchName == "--emoji" || base.IsBooleanValueOption(commandParts, switchName, description);

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "about", "gen-completion", "schema"
    };

    /// <summary>
    /// Pulumi 3.255 renders the env get path as required, although its Cobra validator
    /// accepts either the environment alone or the environment followed by a path.
    /// </summary>
    protected override IEnumerable<string> GetAdditionalUsageSynopses(
        string[] commandPath,
        string helpText)
    {
        if (commandPath is ["pulumi", "env", "get"])
        {
            yield return "pulumi env get [<org-name>/][<project-name>/]<environment-name>[@<version>] [path]";
        }
    }

    /// <inheritdoc />
    protected override IReadOnlyList<CliPositionalArgument> ApplyPositionalArgumentFixes(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments) =>
        commandParts is ["env", "run"]
            ? NormalizeEnvRunArguments(positionalArguments)
            : positionalArguments;

    /// <inheritdoc />
    protected override UsageSynopsisParseResult NormalizeUsageSynopsis(
        CliCommandDefinition command,
        UsageSynopsisParseResult usage) =>
        command.CommandParts is ["env", "run"]
            ? usage with { PositionalArguments = NormalizeEnvRunArguments(usage.PositionalArguments) }
            : usage;

    private static List<CliPositionalArgument> NormalizeEnvRunArguments(
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        var arguments = positionalArguments
            .Select(NormalizeEnvRunArgument)
            .ToList();
        if (arguments.All(argument => !argument.PropertyName.Equals("Args", StringComparison.OrdinalIgnoreCase)))
        {
            arguments.Add(new CliPositionalArgument
            {
                PropertyName = "Args",
                CSharpType = "IEnumerable<string>?",
                Description = "Arguments passed to the command.",
                Phase = CommandLinePhase.Passthrough,
                PositionIndex = 1,
                IsVariadic = true,
            });
        }

        return arguments;
    }

    private static CliPositionalArgument NormalizeEnvRunArgument(CliPositionalArgument argument)
    {
        if (argument.PropertyName.Equals("Command", StringComparison.OrdinalIgnoreCase))
        {
            return argument with
            {
                CSharpType = "string",
                IsRequired = true,
                Phase = CommandLinePhase.Passthrough,
                PositionIndex = 0,
            };
        }

        return argument.PropertyName.Equals("Args", StringComparison.OrdinalIgnoreCase)
            ? argument with
            {
                CSharpType = "IEnumerable<string>?",
                Phase = CommandLinePhase.Passthrough,
                PositionIndex = 1,
                IsVariadic = true,
            }
            : argument;
    }
}
