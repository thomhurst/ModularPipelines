using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "describe-time-based-auto-scaling")]
public record AwsOpsworksDescribeTimeBasedAutoScalingOptions(
[property: CommandSwitch("--instance-ids")] string[] InstanceIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}