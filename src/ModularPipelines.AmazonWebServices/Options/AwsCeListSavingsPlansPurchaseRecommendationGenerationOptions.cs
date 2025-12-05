using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "list-savings-plans-purchase-recommendation-generation")]
public record AwsCeListSavingsPlansPurchaseRecommendationGenerationOptions : AwsOptions
{
    [CliOption("--generation-status")]
    public string? GenerationStatus { get; set; }

    [CliOption("--recommendation-ids")]
    public string[]? RecommendationIds { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}