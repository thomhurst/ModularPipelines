using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "managed", "update-autoscaling")]
public record GcloudComputeInstanceGroupsManagedUpdateAutoscalingOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--cpu-utilization-predictive-method")]
    public string? CpuUtilizationPredictiveMethod { get; set; }

    [CommandSwitch("--max-num-replicas")]
    public string? MaxNumReplicas { get; set; }

    [CommandSwitch("--min-num-replicas")]
    public string? MinNumReplicas { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [BooleanCommandSwitch("--clear-scale-in-control")]
    public bool? ClearScaleInControl { get; set; }

    [CommandSwitch("--scale-in-control")]
    public string[]? ScaleInControl { get; set; }

    [BooleanCommandSwitch("max-scaled-in-replicas")]
    public bool? MaxScaledInReplicas { get; set; }

    [BooleanCommandSwitch("max-scaled-in-replicas-percent")]
    public bool? MaxScaledInReplicasPercent { get; set; }

    [BooleanCommandSwitch("time-window")]
    public bool? TimeWindow { get; set; }

    [CommandSwitch("--disable-schedule")]
    public string? DisableSchedule { get; set; }

    [CommandSwitch("--enable-schedule")]
    public string? EnableSchedule { get; set; }

    [CommandSwitch("--remove-schedule")]
    public string? RemoveSchedule { get; set; }

    [CommandSwitch("--set-schedule")]
    public string? SetSchedule { get; set; }

    [CommandSwitch("--update-schedule")]
    public string? UpdateSchedule { get; set; }

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