using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Podman - daemonless container engine.
/// Podman uses Cobra for its CLI and is compatible with Docker CLI.
///
/// podman help format (podman --help):
/// Manage pods, containers and images
///
/// Usage:
///   podman [options] [command]
///
/// Available Commands:
///   attach      Attach to a running container
///   build       Build an image using instructions from Containerfiles
///   commit      Create new image based on the changed container
///   ...
///
/// Flags:
///   -h, --help   Help for podman
/// </summary>
public partial class PodmanCliScraper : CobraCliScraper
{
    private static readonly IReadOnlyList<CliCompatibilityProperty> NoTruncCompatibilityProperties =
    [
        new CliCompatibilityProperty
        {
            PropertyName = "NoTrunc",
            CSharpType = "bool?",
            ObsoleteMessage = "Podman no longer supports --no-trunc and this property has no effect.",
        },
    ];

    private static readonly IReadOnlyList<CliCompatibilityProperty> SaveKeysCompatibilityProperties =
    [
        new CliCompatibilityProperty
        {
            PropertyName = "SaveKeys",
            CSharpType = "bool?",
            ObsoleteMessage = "Podman no longer supports --save-keys and this property has no effect.",
        },
    ];

    public PodmanCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<PodmanCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "podman";

    public override string NamespacePrefix => "Podman";

    public override string TargetNamespace => "ModularPipelines.Podman";

    public override string OutputDirectory => "src/ModularPipelines.Podman";

    protected virtual string? ComposeProviderPath =>
        Environment.GetEnvironmentVariable("PODMAN_COMPOSE_PROVIDER");

    protected override async Task<string?> GetHelpTextAsync(
        string[] commandPath,
        CancellationToken cancellationToken)
    {
        var composeProvider = ComposeProviderPath;
        if (commandPath.Length < 2
            || !commandPath[1].Equals("compose", StringComparison.OrdinalIgnoreCase)
            || string.IsNullOrWhiteSpace(composeProvider))
        {
            return await base.GetHelpTextAsync(commandPath, cancellationToken);
        }

        var cacheKey = string.Join(" ", commandPath);
        if (HelpCache.TryGet(cacheKey, out var cached))
        {
            return cached;
        }

        var arguments = commandPath.Length > 2
            ? string.Join(" ", commandPath.Skip(2)) + " --help"
            : "--help";
        var result = await Executor.ExecuteAsync(
            composeProvider,
            arguments,
            cancellationToken);
        var helpText = !string.IsNullOrEmpty(result.StandardOutput)
            ? result.StandardOutput
            : result.StandardError;
        if (string.IsNullOrWhiteSpace(helpText))
        {
            Logger.LogWarning("No compose provider help text for command: {Command}", cacheKey);
            return null;
        }

        helpText = NormalizeComposeProviderHelp(helpText);
        if (!IsValidCommandHelp(helpText, [.. commandPath.Skip(1)]))
        {
            var helpPreview = string.Join(
                " | ",
                helpText.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Take(3));
            Logger.LogWarning(
                "Compose provider returned help for the wrong command: {Command}. Help starts: {HelpPreview}",
                cacheKey,
                helpPreview);
            return null;
        }

        HelpCache.Set(cacheKey, helpText);
        return helpText;
    }

    private static string NormalizeComposeProviderHelp(string helpText) =>
        helpText
            .Replace("docker compose", "podman compose", StringComparison.OrdinalIgnoreCase)
            .Replace("docker-compose", "podman compose", StringComparison.OrdinalIgnoreCase)
            .Replace("podman-compose", "podman compose", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "info"
    };

    /// <summary>
    /// Correct Podman 4.x positional metadata where the synopsis does not reflect
    /// the command implementation's required operands.
    /// </summary>
    protected override IReadOnlyList<CliPositionalArgument> ApplyPositionalArgumentFixes(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        return string.Join(' ', commandParts) switch
        {
            "artifact add" => SetRequiredVariadic(positionalArguments, 1, "Path"),
            "container clone" or "pod clone" => SetRequiredCount(positionalArguments, 1),
            "exec" or "container exec" => SetRequiredCount(positionalArguments, 2),
            "artifact rm" or "quadlet rm" => SetRequiredCount(positionalArguments, 0),
            "kube down" or "kube play" => SetOptionalVariadic(
                positionalArguments,
                "Kubefile"),
            "secret exists" or "secret inspect" or "secret rm" => positionalArguments
                .Select(argument => argument with { IsSecret = false })
                .ToList(),
            _ => positionalArguments,
        };
    }

    private static IReadOnlyList<CliPositionalArgument> SetOptionalVariadic(
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        string propertyName) => positionalArguments.Count == 0
        ? positionalArguments
        :
        [
            positionalArguments[0] with
            {
                PropertyName = propertyName,
                CSharpType = "IEnumerable<string>?",
                IsRequired = false,
                IsVariadic = true,
                Description = $"The {propertyName.ToUpperInvariant()} operand.",
            },
        ];

    private static IReadOnlyList<CliPositionalArgument> SetRequiredVariadic(
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        int index,
        string propertyName) => positionalArguments.Count <= index
        ? positionalArguments
        : positionalArguments
            .Select((argument, argumentIndex) => argumentIndex == index
                ? argument with
                {
                    PropertyName = propertyName,
                    CSharpType = "IEnumerable<string>",
                    IsRequired = true,
                    IsVariadic = true,
                    Description = $"The {propertyName.ToUpperInvariant()} operand.",
                }
                : argument)
            .ToList();

    protected override IReadOnlyList<CliCompatibilityProperty> GetCompatibilityProperties(
        string[] commandParts) => string.Join(' ', commandParts) switch
        {
            "generate kube" or "kube generate" => NoTruncCompatibilityProperties,
            "machine rm" => SaveKeysCompatibilityProperties,
            _ => [],
        };

    private static IReadOnlyList<CliPositionalArgument> SetRequiredCount(
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        int requiredCount) =>
        positionalArguments
            .Select((argument, index) => index < requiredCount
                ? argument with
                {
                    CSharpType = argument.CSharpType.TrimEnd('?'),
                    IsRequired = true,
                }
                : argument with
                {
                    CSharpType = $"{argument.CSharpType.TrimEnd('?')}?",
                    IsRequired = false,
                })
            .ToList();
}
