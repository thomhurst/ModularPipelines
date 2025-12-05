using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguru-reviewer", "put-recommendation-feedback")]
public record AwsCodeguruReviewerPutRecommendationFeedbackOptions(
[property: CliOption("--code-review-arn")] string CodeReviewArn,
[property: CliOption("--recommendation-id")] string RecommendationId,
[property: CliOption("--reactions")] string[] Reactions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}