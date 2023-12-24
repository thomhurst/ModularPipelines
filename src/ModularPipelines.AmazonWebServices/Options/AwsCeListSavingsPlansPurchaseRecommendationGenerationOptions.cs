using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "list-savings-plans-purchase-recommendation-generation")]
public record AwsCeListSavingsPlansPurchaseRecommendationGenerationOptions : AwsOptions
{
    [CommandSwitch("--generation-status")]
    public string? GenerationStatus { get; set; }

    [CommandSwitch("--recommendation-ids")]
    public string[]? RecommendationIds { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}