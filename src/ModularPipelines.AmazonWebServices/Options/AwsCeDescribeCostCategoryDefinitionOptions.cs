using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "describe-cost-category-definition")]
public record AwsCeDescribeCostCategoryDefinitionOptions(
[property: CliOption("--cost-category-arn")] string CostCategoryArn
) : AwsOptions
{
    [CliOption("--effective-on")]
    public string? EffectiveOn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}