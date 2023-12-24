using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-recommender-configuration")]
public record AwsPinpointUpdateRecommenderConfigurationOptions(
[property: CommandSwitch("--recommender-id")] string RecommenderId,
[property: CommandSwitch("--update-recommender-configuration")] string UpdateRecommenderConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}