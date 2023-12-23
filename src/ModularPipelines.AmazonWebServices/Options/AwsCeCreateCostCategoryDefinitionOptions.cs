using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "create-cost-category-definition")]
public record AwsCeCreateCostCategoryDefinitionOptions(
[property: CommandSwitch("--name")] string Name,
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

    [CommandSwitch("--resource-tags")]
    public string[]? ResourceTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}