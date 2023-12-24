using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguru-reviewer", "describe-recommendation-feedback")]
public record AwsCodeguruReviewerDescribeRecommendationFeedbackOptions(
[property: CommandSwitch("--code-review-arn")] string CodeReviewArn,
[property: CommandSwitch("--recommendation-id")] string RecommendationId
) : AwsOptions
{
    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}