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
        var result = await ExecuteAndRecordHelpCommandAsync(
            commandPath,
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
            "inspect" => RenamePositionalArgument(positionalArguments, 0, "Container"),
            "manifest add" or "manifest annotate" or "manifest remove" =>
                RenamePositionalArgument(positionalArguments, 1, "Image"),
            "artifact add" => SetPositionalArgument(
                positionalArguments,
                index: 1,
                propertyName: "Path",
                isRequired: true,
                isVariadic: true),
            "container clone" or "pod clone" => SetRequiredCount(positionalArguments, 1),
            "exec" or "container exec" => SetRequiredCount(positionalArguments, 2),
            "artifact rm" or "quadlet rm" => SetPositionalArgument(
                positionalArguments,
                index: 0,
                propertyName: null,
                isRequired: false,
                isVariadic: true),
            "kube down" or "kube play" => PreserveRequiredScalarAndAddVariadic(
                positionalArguments,
                index: 0,
                scalarPropertyName: "Kubefile",
                variadicPropertyName: "AdditionalKubefiles"),
            "secret exists" or "secret inspect" or "secret rm" => positionalArguments
                .Select(argument => argument with { IsSecret = false })
                .ToList(),
            _ => positionalArguments,
        };
    }

    protected override IReadOnlyList<CliOptionDefinition> ApplyOptionFixes(
        string[] commandParts,
        IReadOnlyList<CliOptionDefinition> options)
    {
        var command = string.Join(' ', commandParts);
        if (command is not ("build" or "farm build" or "image build"))
        {
            return options;
        }

        return options.Select(option => (command, option.PropertyName) switch
            {
                ("build" or "image build", "Output") => option with { PropertyName = "Outputs" },
                (_, "Timestamp") => option with { PropertyName = "TimestampValue" },
                _ => option,
            })
            .ToList();
    }

    private static IReadOnlyList<CliPositionalArgument> RenamePositionalArgument(
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        int index,
        string propertyName)
    {
        EnsurePositionalArgumentExists(positionalArguments, index);
        var argument = positionalArguments[index];

        return SetPositionalArgument(
            positionalArguments,
            index,
            propertyName,
            argument.IsRequired,
            argument.IsVariadic);
    }

    private static IReadOnlyList<CliPositionalArgument> SetPositionalArgument(
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        int index,
        string? propertyName,
        bool isRequired,
        bool isVariadic)
    {
        EnsurePositionalArgumentExists(positionalArguments, index);

        return positionalArguments
            .Select((argument, argumentIndex) => argumentIndex == index
                ? ConfigurePositionalArgument(
                    argument,
                    propertyName,
                    isRequired,
                    isVariadic)
                : argument)
            .ToList();
    }

    private static void EnsurePositionalArgumentExists(
        IReadOnlyCollection<CliPositionalArgument> positionalArguments,
        int index)
    {
        if (positionalArguments.Count <= index)
        {
            throw new InvalidDataException(
                $"Podman positional fix expected argument at index {index}, but parsed {positionalArguments.Count}.");
        }
    }

    private static CliPositionalArgument ConfigurePositionalArgument(
        CliPositionalArgument argument,
        string? propertyName,
        bool isRequired,
        bool isVariadic)
    {
        var resolvedPropertyName = propertyName ?? argument.PropertyName;
        var elementType = GetPositionalElementType(argument.CSharpType);
        var csharpType = isVariadic
            ? $"IEnumerable<{elementType}>"
            : elementType;
        return argument with
        {
            PropertyName = resolvedPropertyName,
            CSharpType = isRequired ? csharpType : $"{csharpType}?",
            IsRequired = isRequired,
            IsVariadic = isVariadic,
            Description = propertyName is null
                ? argument.Description
                : $"The {resolvedPropertyName.ToUpperInvariant()} operand.",
        };
    }

    private static IReadOnlyList<CliPositionalArgument> PreserveRequiredScalarAndAddVariadic(
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        int index,
        string scalarPropertyName,
        string variadicPropertyName)
    {
        EnsurePositionalArgumentExists(positionalArguments, index);

        return positionalArguments
            .SelectMany((argument, argumentIndex) => argumentIndex == index
                ? new[]
                {
                    ConfigurePositionalArgument(
                        argument,
                        scalarPropertyName,
                        isRequired: true,
                        isVariadic: false),
                    ConfigurePositionalArgument(
                        argument with { PositionIndex = argument.PositionIndex + 1 },
                        variadicPropertyName,
                        isRequired: false,
                        isVariadic: true),
                }
                : [argumentIndex > index
                    ? argument with { PositionIndex = argument.PositionIndex + 1 }
                    : argument])
            .ToList();
    }

    private static string GetPositionalElementType(string csharpType)
    {
        var nonNullableType = csharpType.TrimEnd('?');
        const string enumerablePrefix = "IEnumerable<";
        return nonNullableType.StartsWith(enumerablePrefix, StringComparison.Ordinal)
               && nonNullableType.EndsWith('>')
            ? nonNullableType[enumerablePrefix.Length..^1]
            : nonNullableType;
    }

    private static IReadOnlyList<CliPositionalArgument> SetRequiredCount(
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        int requiredCount) =>
        positionalArguments
            .Select((argument, index) => ConfigurePositionalArgument(
                argument,
                propertyName: null,
                isRequired: index < requiredCount,
                isVariadic: argument.IsVariadic))
            .ToList();
}
