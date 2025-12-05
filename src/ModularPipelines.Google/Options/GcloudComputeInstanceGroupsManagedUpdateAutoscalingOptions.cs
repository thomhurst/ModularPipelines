using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "update-autoscaling")]
public record GcloudComputeInstanceGroupsManagedUpdateAutoscalingOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--cpu-utilization-predictive-method")]
    public string? CpuUtilizationPredictiveMethod { get; set; }

    [CliOption("--max-num-replicas")]
    public string? MaxNumReplicas { get; set; }

    [CliOption("--min-num-replicas")]
    public string? MinNumReplicas { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliFlag("--clear-scale-in-control")]
    public bool? ClearScaleInControl { get; set; }

    [CliOption("--scale-in-control")]
    public string[]? ScaleInControl { get; set; }

    [CliFlag("max-scaled-in-replicas")]
    public bool? MaxScaledInReplicas { get; set; }

    [CliFlag("max-scaled-in-replicas-percent")]
    public bool? MaxScaledInReplicasPercent { get; set; }

    [CliFlag("time-window")]
    public bool? TimeWindow { get; set; }

    [CliOption("--disable-schedule")]
    public string? DisableSchedule { get; set; }

    [CliOption("--enable-schedule")]
    public string? EnableSchedule { get; set; }

    [CliOption("--remove-schedule")]
    public string? RemoveSchedule { get; set; }

    [CliOption("--set-schedule")]
    public string? SetSchedule { get; set; }

    [CliOption("--update-schedule")]
    public string? UpdateSchedule { get; set; }

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