using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "set-autoscaling")]
public record GcloudComputeInstanceGroupsManagedSetAutoscalingOptions(
[property: CliArgument] string Name,
[property: CliOption("--max-num-replicas")] string MaxNumReplicas
) : GcloudOptions
{
    [CliOption("--cool-down-period")]
    public string? CoolDownPeriod { get; set; }

    [CliOption("--cpu-utilization-predictive-method")]
    public string? CpuUtilizationPredictiveMethod { get; set; }

    [CliOption("--custom-metric-utilization")]
    public string[]? CustomMetricUtilization { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--min-num-replicas")]
    public string? MinNumReplicas { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--remove-stackdriver-metric")]
    public string? RemoveStackdriverMetric { get; set; }

    [CliFlag("--scale-based-on-cpu")]
    public bool? ScaleBasedOnCpu { get; set; }

    [CliFlag("--scale-based-on-load-balancing")]
    public bool? ScaleBasedOnLoadBalancing { get; set; }

    [CliOption("--scale-in-control")]
    public string[]? ScaleInControl { get; set; }

    [CliOption("--set-schedule")]
    public string? SetSchedule { get; set; }

    [CliOption("--stackdriver-metric-filter")]
    public string? StackdriverMetricFilter { get; set; }

    [CliOption("--stackdriver-metric-single-instance-assignment")]
    public string? StackdriverMetricSingleInstanceAssignment { get; set; }

    [CliOption("--stackdriver-metric-utilization-target")]
    public string? StackdriverMetricUtilizationTarget { get; set; }

    [CliOption("--stackdriver-metric-utilization-target-type")]
    public string? StackdriverMetricUtilizationTargetType { get; set; }

    [CliOption("--target-cpu-utilization")]
    public string? TargetCpuUtilization { get; set; }

    [CliOption("--target-load-balancing-utilization")]
    public string? TargetLoadBalancingUtilization { get; set; }

    [CliOption("--update-stackdriver-metric")]
    public string? UpdateStackdriverMetric { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--schedule-cron")]
    public string? ScheduleCron { get; set; }

    [CliOption("--schedule-description")]
    public string? ScheduleDescription { get; set; }

    [CliOption("--schedule-duration-sec")]
    public string? ScheduleDurationSec { get; set; }

    [CliOption("--schedule-min-required-replicas")]
    public string? ScheduleMinRequiredReplicas { get; set; }

    [CliOption("--schedule-time-zone")]
    public string? ScheduleTimeZone { get; set; }
}