using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "list-lens-reviews")]
public record AwsWellarchitectedListLensReviewsOptions(
[property: CommandSwitch("--workload-id")] string WorkloadId
) : AwsOptions
{
    [CommandSwitch("--milestone-number")]
    public int? MilestoneNumber { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}