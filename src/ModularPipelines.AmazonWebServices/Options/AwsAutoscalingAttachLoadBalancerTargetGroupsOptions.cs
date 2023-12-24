using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "attach-load-balancer-target-groups")]
public record AwsAutoscalingAttachLoadBalancerTargetGroupsOptions(
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CommandSwitch("--target-group-arns")] string[] TargetGroupArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}