using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguru-reviewer", "list-recommendation-feedback")]
public record AwsCodeguruReviewerListRecommendationFeedbackOptions(
[property: CliOption("--code-review-arn")] string CodeReviewArn
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--user-ids")]
    public string[]? UserIds { get; set; }

    [CliOption("--recommendation-ids")]
    public string[]? RecommendationIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}