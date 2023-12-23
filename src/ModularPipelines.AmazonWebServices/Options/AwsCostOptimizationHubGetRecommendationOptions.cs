using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cost-optimization-hub", "get-recommendation")]
public record AwsCostOptimizationHubGetRecommendationOptions(
[property: CommandSwitch("--recommendation-id")] string RecommendationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}