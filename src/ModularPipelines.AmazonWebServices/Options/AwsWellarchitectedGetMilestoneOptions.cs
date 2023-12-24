using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "get-milestone")]
public record AwsWellarchitectedGetMilestoneOptions(
[property: CommandSwitch("--workload-id")] string WorkloadId,
[property: CommandSwitch("--milestone-number")] int MilestoneNumber
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}