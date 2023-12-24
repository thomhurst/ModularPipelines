using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "update-recommender")]
public record AwsPersonalizeUpdateRecommenderOptions(
[property: CommandSwitch("--recommender-arn")] string RecommenderArn,
[property: CommandSwitch("--recommender-config")] string RecommenderConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}