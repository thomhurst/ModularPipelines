using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("robomaker", "describe-simulation-job-batch")]
public record AwsRobomakerDescribeSimulationJobBatchOptions(
[property: CommandSwitch("--batch")] string Batch
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}