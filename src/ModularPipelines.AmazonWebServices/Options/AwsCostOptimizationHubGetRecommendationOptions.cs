using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cost-optimization-hub", "get-recommendation")]
public record AwsCostOptimizationHubGetRecommendationOptions(
[property: CliOption("--recommendation-id")] string RecommendationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}