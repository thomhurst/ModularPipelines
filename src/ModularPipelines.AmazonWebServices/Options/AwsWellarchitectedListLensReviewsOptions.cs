using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "list-lens-reviews")]
public record AwsWellarchitectedListLensReviewsOptions(
[property: CliOption("--workload-id")] string WorkloadId
) : AwsOptions
{
    [CliOption("--milestone-number")]
    public int? MilestoneNumber { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}