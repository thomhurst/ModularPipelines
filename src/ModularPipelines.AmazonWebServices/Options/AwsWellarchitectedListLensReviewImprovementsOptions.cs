using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "list-lens-review-improvements")]
public record AwsWellarchitectedListLensReviewImprovementsOptions(
[property: CliOption("--workload-id")] string WorkloadId,
[property: CliOption("--lens-alias")] string LensAlias
) : AwsOptions
{
    [CliOption("--pillar-id")]
    public string? PillarId { get; set; }

    [CliOption("--milestone-number")]
    public int? MilestoneNumber { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--question-priority")]
    public string? QuestionPriority { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}