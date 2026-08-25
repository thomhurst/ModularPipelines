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
public partial class PulumiCliScraper : CobraCliScraper
{
    public PulumiCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<PulumiCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "pulumi";

    public override string NamespacePrefix => "Pulumi";

    public override string TargetNamespace => "ModularPipelines.Pulumi";

    public override string OutputDirectory => "src/ModularPipelines.Pulumi";

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

    private static IReadOnlyList<CliPositionalArgument> NormalizeEnvRunArguments(
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
