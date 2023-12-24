using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "create-recommender-configuration")]
public record AwsPinpointCreateRecommenderConfigurationOptions(
[property: CommandSwitch("--create-recommender-configuration")] string CreateRecommenderConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}