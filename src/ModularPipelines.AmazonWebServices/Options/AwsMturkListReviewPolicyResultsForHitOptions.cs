using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "list-review-policy-results-for-hit")]
public record AwsMturkListReviewPolicyResultsForHitOptions(
[property: CommandSwitch("--hit-id")] string HitId
) : AwsOptions
{
    [CommandSwitch("--policy-levels")]
    public string[]? PolicyLevels { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}