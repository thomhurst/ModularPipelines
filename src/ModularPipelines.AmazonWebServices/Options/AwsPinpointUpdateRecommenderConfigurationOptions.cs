using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-recommender-configuration")]
public record AwsPinpointUpdateRecommenderConfigurationOptions(
[property: CliOption("--recommender-id")] string RecommenderId,
[property: CliOption("--update-recommender-configuration")] string UpdateRecommenderConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}