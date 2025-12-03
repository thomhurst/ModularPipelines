using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "get-milestone")]
public record AwsWellarchitectedGetMilestoneOptions(
[property: CliOption("--workload-id")] string WorkloadId,
[property: CliOption("--milestone-number")] int MilestoneNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}