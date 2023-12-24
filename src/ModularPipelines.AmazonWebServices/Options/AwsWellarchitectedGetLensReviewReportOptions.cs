using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "get-lens-review-report")]
public record AwsWellarchitectedGetLensReviewReportOptions(
[property: CommandSwitch("--workload-id")] string WorkloadId,
[property: CommandSwitch("--lens-alias")] string LensAlias
) : AwsOptions
{
    [CommandSwitch("--milestone-number")]
    public int? MilestoneNumber { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}