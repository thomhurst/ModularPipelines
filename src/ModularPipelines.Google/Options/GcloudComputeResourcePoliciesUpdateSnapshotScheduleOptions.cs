using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "resource-policies", "update", "snapshot-schedule")]
public record GcloudComputeResourcePoliciesUpdateSnapshotScheduleOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--max-retention-days")]
    public string? MaxRetentionDays { get; set; }

    [CommandSwitch("--on-source-disk-delete")]
    public string? OnSourceDiskDelete { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--snapshot-labels")]
    public IEnumerable<KeyValue>? SnapshotLabels { get; set; }

    [CommandSwitch("--weekly-schedule-from-file")]
    public string? WeeklyScheduleFromFile { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [BooleanCommandSwitch("--daily-schedule")]
    public bool? DailySchedule { get; set; }

    [CommandSwitch("--hourly-schedule")]
    public string? HourlySchedule { get; set; }

    [CommandSwitch("--weekly-schedule")]
    public string? WeeklySchedule { get; set; }
}