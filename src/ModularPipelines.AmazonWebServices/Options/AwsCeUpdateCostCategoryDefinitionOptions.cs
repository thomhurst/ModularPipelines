using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "update-cost-category-definition")]
public record AwsCeUpdateCostCategoryDefinitionOptions(
[property: CliOption("--cost-category-arn")] string CostCategoryArn,
[property: CliOption("--rule-version")] string RuleVersion,
[property: CliOption("--rules")] string[] Rules
) : AwsOptions
{
    [CliOption("--effective-start")]
    public string? EffectiveStart { get; set; }

    [CliOption("--default-value")]
    public string? DefaultValue { get; set; }

    [CliOption("--split-charge-rules")]
    public string[]? SplitChargeRules { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}