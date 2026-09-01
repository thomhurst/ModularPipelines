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
public partial class KustomizeCliScraper : CobraCliScraper
{
    public KustomizeCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<KustomizeCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "kustomize";

    public override string NamespacePrefix => "Kustomize";

    public override string TargetNamespace => "ModularPipelines.Kubernetes";

    public override string OutputDirectory => "src/ModularPipelines.Kubernetes";

    protected override string VersionArguments => "version";

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
    protected override string? GetCollectionSeparator(
        string[] commandParts,
        string switchName,
        string typeHint,
        string description) =>
        IsCreateScalarMapOption(commandParts, switchName, typeHint)
            ? ","
            : base.GetCollectionSeparator(commandParts, switchName, typeHint, description);

    protected override bool IsRepeatableOption(
        string[] commandParts,
        string switchName,
        string typeHint,
        string description,
        string helpText) =>
        IsCreateScalarMapOption(commandParts, switchName, typeHint)
        || base.IsRepeatableOption(commandParts, switchName, typeHint, description, helpText);

    private static bool IsCreateScalarMapOption(
        IReadOnlyList<string> commandParts,
        string switchName,
        string typeHint) =>
        commandParts is ["create"]
        && switchName is "--annotations" or "--labels"
        && typeHint.Equals("string", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Kustomize validates buildmetadata operands but omits them from Cobra's Use value.
    /// </summary>
    protected override IEnumerable<string> GetAdditionalUsageSynopses(
        string[] commandPath,
        string helpText) =>
        commandPath.Skip(1).ToArray() is [.., "buildmetadata"]
            ? [$"{string.Join(" ", commandPath)} <metadata>"]
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
