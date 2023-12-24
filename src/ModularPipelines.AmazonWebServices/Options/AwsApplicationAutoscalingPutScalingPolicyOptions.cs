using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-autoscaling", "put-scaling-policy")]
public record AwsApplicationAutoscalingPutScalingPolicyOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--service-namespace")] string ServiceNamespace,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--scalable-dimension")] string ScalableDimension
) : AwsOptions
{
    [CommandSwitch("--policy-type")]
    public string? PolicyType { get; set; }

    [CommandSwitch("--step-scaling-policy-configuration")]
    public string? StepScalingPolicyConfiguration { get; set; }

    [CommandSwitch("--target-tracking-scaling-policy-configuration")]
    public string? TargetTrackingScalingPolicyConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}