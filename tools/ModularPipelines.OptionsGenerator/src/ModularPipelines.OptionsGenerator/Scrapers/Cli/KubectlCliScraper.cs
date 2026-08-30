using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for kubectl.
/// kubectl is a Cobra-based CLI with consistent help formatting.
/// </summary>
public class KubectlCliScraper : CobraCliScraper
{
    public override string ToolName => "kubectl";
    public override string NamespacePrefix => "Kubernetes";
    public override string TargetNamespace => "ModularPipelines.Kubernetes";
    public override string OutputDirectory => "src/ModularPipelines.Kubernetes";

    protected override string VersionArguments => "version --client";

    public KubectlCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<KubectlCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    protected override UsageSynopsisParseResult NormalizeUsageSynopsis(
        CliCommandDefinition command,
        UsageSynopsisParseResult usage)
    {
        var positionalArguments = ApplyPositionalArgumentFixes(
            command.CommandParts,
            GetPositionalArguments(usage, command.Options));
        var unparsedOperandTokens = command.CommandParts is ["events"]
            ? []
            : usage.UnparsedOperandTokens;
        return usage with
        {
            HasOperandTokens = positionalArguments.Count > 0
                               || unparsedOperandTokens.Count > 0,
            PositionalArguments = positionalArguments,
            UnparsedOperandTokens = unparsedOperandTokens,
        };
    }

    /// <summary>
    /// kubectl has some additional skip patterns for plugin and completion commands.
    /// </summary>
    protected override bool IsSkippableSubcommand(string subcommand)
    {
        if (base.IsSkippableSubcommand(subcommand))
        {
            return true;
        }

        var lowerName = subcommand.ToLowerInvariant();
        return lowerName is "plugin" or "kustomize" or "api-versions" or "api-resources";
    }

    protected override IReadOnlyList<CliPositionalArgument> ApplyPositionalArgumentFixes(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments) =>
        commandParts switch
        {
            ["annotate"] => AllowOmittedValue(
                CollapseNumberedRepeat(
                    positionalArguments,
                    "Key_1Val_1",
                    "KeyNValN",
                    "Annotations"),
                "Annotations"),
            ["auth", "can-i"] => AllowOmittedValue(positionalArguments, "Verb"),
            ["cordon" or "drain" or "uncordon"] => AllowOmittedValue(positionalArguments, "Node"),
            ["debug"] => NormalizeDebugArguments(positionalArguments),
            ["events"] => RemoveArgument(positionalArguments, "O"),
            ["label"] => NormalizeLabelArguments(positionalArguments),
            ["logs"] => AllowOmittedValue(positionalArguments, "Pod"),
            ["port-forward"] => CollapseNumberedRepeat(
                positionalArguments,
                "LocalPortRemotePort",
                "LocalPortNRemotePortN",
                "LocalPortRemotePort"),
            ["rollout", "history" or "pause" or "restart" or "resume" or "status" or "undo"] =>
                AllowOmittedValue(positionalArguments, "Resource", "TypeName"),
            ["taint"] => NormalizeTaintArguments(positionalArguments),
            _ => positionalArguments,
        };

    private static IReadOnlyList<CliPositionalArgument> NormalizeDebugArguments(
        IReadOnlyList<CliPositionalArgument> arguments)
    {
        var combined = arguments.FirstOrDefault(argument =>
            argument.PropertyName.Equals("CommandArgs", StringComparison.OrdinalIgnoreCase));
        if (combined is null)
        {
            return arguments;
        }

        return CliPositionalArgument.MergeDuplicates(
        [
            .. arguments.Where(argument => !ReferenceEquals(argument, combined)),
            combined with
            {
                IsVariadic = false,
                IsValidationRequired = false,
            },
            combined with
            {
                PropertyName = "Args",
                CSharpType = "IEnumerable<string>?",
                PositionIndex = combined.PositionIndex + 1,
                IsRequired = false,
                IsVariadic = true,
                PrependOptionTerminator = false,
            },
        ]);
    }

    private static IReadOnlyList<CliPositionalArgument> NormalizeLabelArguments(
        IReadOnlyList<CliPositionalArgument> arguments) =>
        arguments
            .Select(argument => argument.PropertyName switch
            {
                "Key_1Val_1" => argument with
                {
                    CSharpType = "IEnumerable<string>",
                    IsRequired = true,
                    IsVariadic = true,
                },
                "KeyNValN" => argument with
                {
                    IsValidationRequired = false,
                },
                _ => argument,
            })
            .ToArray();

    private static IReadOnlyList<CliPositionalArgument> NormalizeTaintArguments(
        IReadOnlyList<CliPositionalArgument> arguments) =>
        AllowOmittedValue(
            CollapseNumberedRepeat(
                arguments,
                "Key_1Val_1TaintEffect_1",
                "KeyNValNTaintEffectN",
                "Taints"),
            "Name");

    private static IReadOnlyList<CliPositionalArgument> AllowOmittedValue(
        IReadOnlyList<CliPositionalArgument> arguments,
        params string[] propertyNames) =>
        arguments
            .Select(argument => propertyNames.Contains(
                argument.PropertyName,
                StringComparer.OrdinalIgnoreCase)
                ? argument with
                {
                    IsValidationRequired = false,
                }
                : argument)
            .ToArray();

    private static IReadOnlyList<CliPositionalArgument> RemoveArgument(
        IReadOnlyList<CliPositionalArgument> arguments,
        string propertyName) =>
        CliPositionalArgument.MergeDuplicates(arguments.Where(argument =>
            !argument.PropertyName.Equals(propertyName, StringComparison.OrdinalIgnoreCase)));

    private static IReadOnlyList<CliPositionalArgument> CollapseNumberedRepeat(
        IReadOnlyList<CliPositionalArgument> arguments,
        string firstPropertyName,
        string repeatedPropertyName,
        string generatedPropertyName)
    {
        var normalized = arguments
            .Where(argument => !argument.PropertyName.Equals(
                repeatedPropertyName,
                StringComparison.OrdinalIgnoreCase))
            .Select(argument => argument.PropertyName.Equals(
                firstPropertyName,
                StringComparison.OrdinalIgnoreCase)
                ? argument with
                {
                    PropertyName = generatedPropertyName,
                    CSharpType = argument.IsRequired
                        ? "IEnumerable<string>"
                        : "IEnumerable<string>?",
                    IsVariadic = true,
                }
                : argument);
        return CliPositionalArgument.MergeDuplicates(normalized);
    }
}
