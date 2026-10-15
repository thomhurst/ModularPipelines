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
    private const string GlobalOptionsHeading = "The following options can be passed to any command";
    private const string CombinedResourcePattern = @"\A[^/\s]+/[^/\s]+\z";

    private static readonly string[] RequiredGlobalSwitches = ["--namespace", "--context", "--kubeconfig"];

    public override string ToolName => "kubectl";
    public override string NamespacePrefix => "Kubernetes";
    public override string TargetNamespace => "ModularPipelines.Kubernetes";
    public override string OutputDirectory => "src/ModularPipelines.Kubernetes";

    /// <inheritdoc />
    public override CliToolDefinition CreateToolDefinition() => base.CreateToolDefinition() with
    {
        CommandCoverage = new CliCommandCoveragePolicy
        {
            SentinelCommands =
            [
                "kubectl get",
                "kubectl create",
                "kubectl config current-context",
                "kubectl config get-clusters",
                "kubectl config get-users",
            ],
        },
    };

    protected override string VersionArguments => "version --client";

    protected override bool GlobalOptionsBeforeSubcommands => false;

    protected override string GetHelpArguments(string[] commandPath) =>
        commandPath is [_, "options"] ? "options" : base.GetHelpArguments(commandPath);

    protected override bool ShouldAcceptHelpResult(IReadOnlyList<string> commandPath, CliCommandResult result) =>
        commandPath is [_, "options"]
            ? !result.Unavailable && result.ExitCode == 0
            : base.ShouldAcceptHelpResult(commandPath, result);

    protected override async Task<string?> GetHelpTextAsync(string[] commandPath, CancellationToken cancellationToken)
    {
        var help = await base.GetHelpTextAsync(commandPath, cancellationToken).ConfigureAwait(false);
        if (commandPath.Length != 1 || help is null)
        {
            return help;
        }

        var globalHelp = await base.GetHelpTextAsync([ToolName, "options"], cancellationToken).ConfigureAwait(false);
        if (globalHelp is null || !globalHelp.Contains(GlobalOptionsHeading + ":", StringComparison.Ordinal)
            || RequiredGlobalSwitches.Except(
                ParseGlobalOptions(globalHelp).Select(option => option.SwitchName), StringComparer.Ordinal).Any())
        {
            throw new InvalidOperationException("kubectl global option help is unavailable or incomplete.");
        }

        return help + "\n" + globalHelp;
    }

    protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText) =>
        ApplyOptionFixes([], ParseNamedOptionSection(helpText, GlobalOptionsHeading, []));

    protected override string NormalizeOptionDescription(string description) =>
        description.StartsWith("Default cache directory", StringComparison.Ordinal)
            ? "Default cache directory."
            : description;

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
        CliCommandDefinition command, IReadOnlyList<UsageRequiredAlternativeGroup> groups)
    {
        string[] selectionSwitches = command.CommandParts switch
        {
            ["annotate" or "label"] => ["--all", "--selector", "--field-selector"],
            ["scale"] => ["--all", "--selector"],
            _ => [],
        };
        var selectionOptions = command.Options.Where(option =>
            selectionSwitches.Contains(option.SwitchName, StringComparer.Ordinal)).ToArray();
        return [.. groups.Select(group => NormalizeResourceInputs(group, command.Options, selectionOptions))];
    }

    // Resource input can use TYPE NAME, TYPE/NAME, selectors, filenames, or a kustomization.
    // A bare TYPE still needs a name or selector; a combined TYPE/NAME supplies both.
    private static UsageRequiredAlternativeGroup NormalizeResourceInputs(
        UsageRequiredAlternativeGroup group, IReadOnlyList<CliOptionDefinition> options,
        IReadOnlyList<CliOptionDefinition> selectionOptions)
    {
        var members = group.Members.Select(NormalizeCombinedResource).ToList();
        var groups = group.Groups.Select(child => NormalizeResourceInputs(child, options, selectionOptions)).ToList();
        if (!group.IsChoice
            && members.FirstOrDefault(member => member.PositionalPropertyName == "Type") is { } type
            && members.FirstOrDefault(member => member.PositionalPropertyName == "Name") is { } name)
        {
            members.Remove(name);
            groups.Add(CreateResourceSelection(type, name, selectionOptions));
        }

        if (group.IsChoice && members.Any(member => member.OptionSwitch is "-f" or "--filename")
            && options.Any(option => option.SwitchName == "--kustomize"))
        {
            members.Add(new UsageRequiredAlternativeMember { OptionSwitch = "--kustomize" });
        }

        return group with
        {
            Members = members,
            Groups = groups,
        };
    }

    private static UsageRequiredAlternativeMember NormalizeCombinedResource(UsageRequiredAlternativeMember member) =>
        member.PositionalPropertyName == "TypeOrName"
            ? member with { ValuePattern = CombinedResourcePattern }
            : member;

    private static UsageRequiredAlternativeGroup CreateResourceSelection(
        UsageRequiredAlternativeMember type, UsageRequiredAlternativeMember name,
        IReadOnlyList<CliOptionDefinition> options) => new()
        {
            Members =
            [
                name,
                type with { ValuePattern = CombinedResourcePattern },
                .. options.Select(option => new UsageRequiredAlternativeMember
                    {
                        OptionSwitch = option.SwitchName,
                        RequiredFlagValue = option.SwitchName == "--all" ? true : null,
                    }),
            ],
        };

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
