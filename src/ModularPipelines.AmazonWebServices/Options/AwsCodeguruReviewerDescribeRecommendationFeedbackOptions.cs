using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguru-reviewer", "describe-recommendation-feedback")]
public record AwsCodeguruReviewerDescribeRecommendationFeedbackOptions(
[property: CliOption("--code-review-arn")] string CodeReviewArn,
[property: CliOption("--recommendation-id")] string RecommendationId
) : AwsOptions
{
    [CliOption("--user-id")]
    public string? UserId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}