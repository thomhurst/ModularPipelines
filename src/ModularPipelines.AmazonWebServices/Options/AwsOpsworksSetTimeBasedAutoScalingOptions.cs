using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "set-time-based-auto-scaling")]
public record AwsOpsworksSetTimeBasedAutoScalingOptions(
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--auto-scaling-schedule")]
    public string? AutoScalingSchedule { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}