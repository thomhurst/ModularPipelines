using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "detach-load-balancer-target-groups")]
public record AwsAutoscalingDetachLoadBalancerTargetGroupsOptions(
[property: CliOption("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CliOption("--target-group-arns")] string[] TargetGroupArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}