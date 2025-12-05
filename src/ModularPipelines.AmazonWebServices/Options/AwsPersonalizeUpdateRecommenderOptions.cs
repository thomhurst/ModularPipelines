using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "update-recommender")]
public record AwsPersonalizeUpdateRecommenderOptions(
[property: CliOption("--recommender-arn")] string RecommenderArn,
[property: CliOption("--recommender-config")] string RecommenderConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}