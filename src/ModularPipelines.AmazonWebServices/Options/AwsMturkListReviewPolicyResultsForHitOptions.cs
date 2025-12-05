using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "list-review-policy-results-for-hit")]
public record AwsMturkListReviewPolicyResultsForHitOptions(
[property: CliOption("--hit-id")] string HitId
) : AwsOptions
{
    [CliOption("--policy-levels")]
    public string[]? PolicyLevels { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}