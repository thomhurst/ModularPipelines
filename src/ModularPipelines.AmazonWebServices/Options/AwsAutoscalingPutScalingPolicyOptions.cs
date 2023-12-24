using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "put-scaling-policy")]
public record AwsAutoscalingPutScalingPolicyOptions(
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CommandSwitch("--policy-name")] string PolicyName
) : AwsOptions
{
    [CommandSwitch("--policy-type")]
    public string? PolicyType { get; set; }

    [CommandSwitch("--adjustment-type")]
    public string? AdjustmentType { get; set; }

    [CommandSwitch("--min-adjustment-step")]
    public int? MinAdjustmentStep { get; set; }

    [CommandSwitch("--min-adjustment-magnitude")]
    public int? MinAdjustmentMagnitude { get; set; }

    [CommandSwitch("--scaling-adjustment")]
    public int? ScalingAdjustment { get; set; }

    [CommandSwitch("--cooldown")]
    public int? Cooldown { get; set; }

    [CommandSwitch("--metric-aggregation-type")]
    public string? MetricAggregationType { get; set; }

    [CommandSwitch("--step-adjustments")]
    public string[]? StepAdjustments { get; set; }

    [CommandSwitch("--estimated-instance-warmup")]
    public int? EstimatedInstanceWarmup { get; set; }

    [CommandSwitch("--target-tracking-configuration")]
    public string? TargetTrackingConfiguration { get; set; }

    [CommandSwitch("--predictive-scaling-configuration")]
    public string? PredictiveScalingConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}