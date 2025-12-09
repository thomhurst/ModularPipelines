using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "create-recommender-configuration")]
public record AwsPinpointCreateRecommenderConfigurationOptions(
[property: CliOption("--create-recommender-configuration")] string CreateRecommenderConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}