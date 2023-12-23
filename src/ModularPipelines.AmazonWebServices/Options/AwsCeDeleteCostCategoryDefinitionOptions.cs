using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "delete-cost-category-definition")]
public record AwsCeDeleteCostCategoryDefinitionOptions(
[property: CommandSwitch("--cost-category-arn")] string CostCategoryArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}