using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "managed", "set-autoscaling")]
public record GcloudComputeInstanceGroupsManagedSetAutoscalingOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--max-num-replicas")] string MaxNumReplicas
) : GcloudOptions
{
    [CommandSwitch("--cool-down-period")]
    public string? CoolDownPeriod { get; set; }

    [CommandSwitch("--cpu-utilization-predictive-method")]
    public string? CpuUtilizationPredictiveMethod { get; set; }

    [CommandSwitch("--custom-metric-utilization")]
    public string[]? CustomMetricUtilization { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--min-num-replicas")]
    public string? MinNumReplicas { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--remove-stackdriver-metric")]
    public string? RemoveStackdriverMetric { get; set; }

    [BooleanCommandSwitch("--scale-based-on-cpu")]
    public bool? ScaleBasedOnCpu { get; set; }

    [BooleanCommandSwitch("--scale-based-on-load-balancing")]
    public bool? ScaleBasedOnLoadBalancing { get; set; }

    [CommandSwitch("--scale-in-control")]
    public string[]? ScaleInControl { get; set; }

    [CommandSwitch("--set-schedule")]
    public string? SetSchedule { get; set; }

    [CommandSwitch("--stackdriver-metric-filter")]
    public string? StackdriverMetricFilter { get; set; }

    [CommandSwitch("--stackdriver-metric-single-instance-assignment")]
    public string? StackdriverMetricSingleInstanceAssignment { get; set; }

    [CommandSwitch("--stackdriver-metric-utilization-target")]
    public string? StackdriverMetricUtilizationTarget { get; set; }

    [CommandSwitch("--stackdriver-metric-utilization-target-type")]
    public string? StackdriverMetricUtilizationTargetType { get; set; }

    [CommandSwitch("--target-cpu-utilization")]
    public string? TargetCpuUtilization { get; set; }

    [CommandSwitch("--target-load-balancing-utilization")]
    public string? TargetLoadBalancingUtilization { get; set; }

    [CommandSwitch("--update-stackdriver-metric")]
    public string? UpdateStackdriverMetric { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--schedule-cron")]
    public string? ScheduleCron { get; set; }

    [CommandSwitch("--schedule-description")]
    public string? ScheduleDescription { get; set; }

    [CommandSwitch("--schedule-duration-sec")]
    public string? ScheduleDurationSec { get; set; }

    [CommandSwitch("--schedule-min-required-replicas")]
    public string? ScheduleMinRequiredReplicas { get; set; }

    [CommandSwitch("--schedule-time-zone")]
    public string? ScheduleTimeZone { get; set; }
}