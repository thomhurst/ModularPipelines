using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "put-auto-scaling-policy")]
public record AwsEmrPutAutoScalingPolicyOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--instance-group-id")] string InstanceGroupId,
[property: CommandSwitch("--auto-scaling-policy")] string AutoScalingPolicy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}