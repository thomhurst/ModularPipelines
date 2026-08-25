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
        var positionalArguments = GetPositionalArguments(usage, command.Options);
        return usage with
        {
            HasOperandTokens = positionalArguments.Count > 0
                               || usage.UnparsedOperandTokens.Count > 0,
            PositionalArguments = positionalArguments,
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
            ["annotate"] => CollapseNumberedRepeat(
                positionalArguments,
                "Key_1Val_1",
                "KeyNValN",
                "Annotations"),
            ["port-forward"] => CollapseNumberedRepeat(
                positionalArguments,
                "LocalPortRemotePort",
                "LocalPortNRemotePortN",
                "LocalPortRemotePort"),
            ["taint"] => CollapseNumberedRepeat(
                positionalArguments,
                "Key_1Val_1TaintEffect_1",
                "KeyNValNTaintEffectN",
                "Taints"),
            _ => positionalArguments,
        };

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
