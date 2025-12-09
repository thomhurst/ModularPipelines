using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "get-lens-review-report")]
public record AwsWellarchitectedGetLensReviewReportOptions(
[property: CliOption("--workload-id")] string WorkloadId,
[property: CliOption("--lens-alias")] string LensAlias
) : AwsOptions
{
    [CliOption("--milestone-number")]
    public int? MilestoneNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}