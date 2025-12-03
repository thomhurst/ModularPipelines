using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "put-scaling-policy")]
public record AwsAutoscalingPutScalingPolicyOptions(
[property: CliOption("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CliOption("--policy-name")] string PolicyName
) : AwsOptions
{
    [CliOption("--policy-type")]
    public string? PolicyType { get; set; }

    [CliOption("--adjustment-type")]
    public string? AdjustmentType { get; set; }

    [CliOption("--min-adjustment-step")]
    public int? MinAdjustmentStep { get; set; }

    [CliOption("--min-adjustment-magnitude")]
    public int? MinAdjustmentMagnitude { get; set; }

    [CliOption("--scaling-adjustment")]
    public int? ScalingAdjustment { get; set; }

    [CliOption("--cooldown")]
    public int? Cooldown { get; set; }

    [CliOption("--metric-aggregation-type")]
    public string? MetricAggregationType { get; set; }

    [CliOption("--step-adjustments")]
    public string[]? StepAdjustments { get; set; }

    [CliOption("--estimated-instance-warmup")]
    public int? EstimatedInstanceWarmup { get; set; }

    [CliOption("--target-tracking-configuration")]
    public string? TargetTrackingConfiguration { get; set; }

    [CliOption("--predictive-scaling-configuration")]
    public string? PredictiveScalingConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}