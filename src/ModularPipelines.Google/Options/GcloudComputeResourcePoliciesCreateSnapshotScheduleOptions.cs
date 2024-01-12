using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "resource-policies", "create", "snapshot-schedule")]
public record GcloudComputeResourcePoliciesCreateSnapshotScheduleOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--max-retention-days")] string MaxRetentionDays,
[property: CommandSwitch("--weekly-schedule-from-file")] string WeeklyScheduleFromFile,
[property: CommandSwitch("--start-time")] string StartTime,
[property: BooleanCommandSwitch("--daily-schedule")] bool DailySchedule,
[property: CommandSwitch("--hourly-schedule")] string HourlySchedule,
[property: CommandSwitch("--weekly-schedule")] string WeeklySchedule
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--on-source-disk-delete")]
    public string? OnSourceDiskDelete { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [BooleanCommandSwitch("--guest-flush")]
    public bool? GuestFlush { get; set; }

    [CommandSwitch("--snapshot-labels")]
    public IEnumerable<KeyValue>? SnapshotLabels { get; set; }

    [CommandSwitch("--storage-location")]
    public string? StorageLocation { get; set; }
}