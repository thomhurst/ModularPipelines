using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for kubectl.
/// kubectl is a Cobra-based CLI with consistent help formatting.
/// </summary>
public class KubectlCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<KubectlCliScraper> logger) : CobraCliScraper(executor, helpCache, logger)
{
    public override string ToolName => "kubectl";
    public override string NamespacePrefix => "Kubernetes";
    public override string TargetNamespace => "ModularPipelines.Kubernetes";
    public override string OutputDirectory => "src/ModularPipelines.Kubernetes";

    protected override string VersionArguments => "version --client";

    protected override IReadOnlyList<CliOptionDefinition> ApplyOptionFixes(
        string[] commandParts,
        IReadOnlyList<CliOptionDefinition> options) =>
        [.. base.ApplyOptionFixes(commandParts, options).Select(option =>
            // kubectl's pflag Booleans accept bare switches and explicit false regardless
            // of the default, which can differ between generator and consumer platforms.
            option.CSharpType == "bool?"
                ? option with { IsFlag = true, NegatedSwitchName = option.SwitchName + "=false" }
                : option)];

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

    protected override IReadOnlyList<UsageRequiredAlternativeGroup> NormalizeRequiredAlternativeGroups(
        CliCommandDefinition command, IReadOnlyList<UsageRequiredAlternativeGroup> groups) =>
        [.. groups.Select(group => NormalizeResourceInputs(group, command.Options))];

    // Resource input can use TYPE NAME, TYPE/NAME, selectors, filenames, or a kustomization.
    // kubectl validates the resource string itself; a separate NAME is not always present.
    private static UsageRequiredAlternativeGroup NormalizeResourceInputs(
        UsageRequiredAlternativeGroup group, IReadOnlyList<CliOptionDefinition> options)
    {
        var members = group.Members;
        if (!group.IsChoice && members.Any(member => member.PositionalPropertyName == "Type"))
        {
            members = [.. members.Where(member => member.PositionalPropertyName != "Name")];
        }

        if (group.IsChoice && members.Any(member => member.OptionSwitch is "-f" or "--filename")
            && options.Any(option => option.SwitchName == "--kustomize"))
        {
            members = [.. members, new UsageRequiredAlternativeMember { OptionSwitch = "--kustomize" }];
        }

        return group with
        {
            Members = members,
            Groups = [.. group.Groups.Select(child => NormalizeResourceInputs(child, options))],
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
                RenameArgument(positionalArguments, "KeyVal", "Annotations"),
                "Annotations"),
            ["auth", "can-i"] => AllowOmittedValue(positionalArguments, "Verb"),
            ["cordon" or "drain" or "uncordon"] => AllowOmittedValue(positionalArguments, "Node"),
            ["debug"] => AllowOmittedValue(
                NormalizeDebugArguments(positionalArguments),
                "Pod"),
            ["events"] => RemoveArgument(positionalArguments, "O"),
            ["exec"] => AllowOmittedValue(positionalArguments, "Pod"),
            ["label"] => AllowOmittedValue(RenameArgument(positionalArguments, "KeyVal", "Labels"), "Labels"),
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

    private static IReadOnlyList<CliPositionalArgument> RenameArgument(
        IReadOnlyList<CliPositionalArgument> arguments, string sourceName, string targetName) =>
        [.. arguments.Select(argument => argument.PropertyName == sourceName
            ? argument with { PropertyName = targetName }
            : argument)];

    private static IReadOnlyList<CliPositionalArgument> NormalizeTaintArguments(
        IReadOnlyList<CliPositionalArgument> arguments) =>
        AllowOmittedValue(
            RenameArgument(arguments, "KeyValTaintEffect", "Taints"),
            "Name");

    private static IReadOnlyList<CliPositionalArgument> AllowOmittedValue(
        IReadOnlyList<CliPositionalArgument> arguments,
        params string[] propertyNames) =>
        [.. arguments
            .Select(argument => propertyNames.Contains(
                argument.PropertyName,
                StringComparer.OrdinalIgnoreCase)
                ? argument with
                {
                    IsValidationRequired = false,
                }
                : argument)];

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
