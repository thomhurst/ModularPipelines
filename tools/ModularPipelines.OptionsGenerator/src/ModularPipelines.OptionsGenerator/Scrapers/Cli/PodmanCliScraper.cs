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
        var requiredCount = string.Join(' ', commandParts) switch
        {
            "container clone" or "pod clone" => 1,
            "exec" => 2,
            _ => (int?) null,
        };

        if (requiredCount is null)
        {
            return positionalArguments;
        }

        return positionalArguments
            .Select((argument, index) => index < requiredCount.Value
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
}
