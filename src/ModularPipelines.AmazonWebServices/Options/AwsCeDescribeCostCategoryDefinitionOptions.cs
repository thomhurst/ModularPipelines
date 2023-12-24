using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "describe-cost-category-definition")]
public record AwsCeDescribeCostCategoryDefinitionOptions(
[property: CommandSwitch("--cost-category-arn")] string CostCategoryArn
) : AwsOptions
{
    [CommandSwitch("--effective-on")]
    public string? EffectiveOn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}