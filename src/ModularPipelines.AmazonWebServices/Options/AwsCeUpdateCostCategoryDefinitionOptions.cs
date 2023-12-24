using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "update-cost-category-definition")]
public record AwsCeUpdateCostCategoryDefinitionOptions(
[property: CommandSwitch("--cost-category-arn")] string CostCategoryArn,
[property: CommandSwitch("--rule-version")] string RuleVersion,
[property: CommandSwitch("--rules")] string[] Rules
) : AwsOptions
{
    [CommandSwitch("--effective-start")]
    public string? EffectiveStart { get; set; }

    [CommandSwitch("--default-value")]
    public string? DefaultValue { get; set; }

    [CommandSwitch("--split-charge-rules")]
    public string[]? SplitChargeRules { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}