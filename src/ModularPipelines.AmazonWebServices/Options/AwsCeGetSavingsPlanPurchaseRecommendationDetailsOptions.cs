using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "get-savings-plan-purchase-recommendation-details")]
public record AwsCeGetSavingsPlanPurchaseRecommendationDetailsOptions(
[property: CommandSwitch("--recommendation-detail-id")] string RecommendationDetailId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}