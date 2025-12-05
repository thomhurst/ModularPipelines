using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "get-savings-plan-purchase-recommendation-details")]
public record AwsCeGetSavingsPlanPurchaseRecommendationDetailsOptions(
[property: CliOption("--recommendation-detail-id")] string RecommendationDetailId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}