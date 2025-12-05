using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "delete-cost-category-definition")]
public record AwsCeDeleteCostCategoryDefinitionOptions(
[property: CliOption("--cost-category-arn")] string CostCategoryArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}