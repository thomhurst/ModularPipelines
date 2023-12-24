using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguru-reviewer", "put-recommendation-feedback")]
public record AwsCodeguruReviewerPutRecommendationFeedbackOptions(
[property: CommandSwitch("--code-review-arn")] string CodeReviewArn,
[property: CommandSwitch("--recommendation-id")] string RecommendationId,
[property: CommandSwitch("--reactions")] string[] Reactions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}