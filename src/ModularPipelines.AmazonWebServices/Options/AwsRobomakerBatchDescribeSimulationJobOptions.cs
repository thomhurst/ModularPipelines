using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("robomaker", "batch-describe-simulation-job")]
public record AwsRobomakerBatchDescribeSimulationJobOptions(
[property: CommandSwitch("--jobs")] string[] Jobs
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}