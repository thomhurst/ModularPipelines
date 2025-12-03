using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "resource-policies", "create", "snapshot-schedule")]
public record GcloudComputeResourcePoliciesCreateSnapshotScheduleOptions(
[property: CliArgument] string Name,
[property: CliOption("--max-retention-days")] string MaxRetentionDays,
[property: CliOption("--weekly-schedule-from-file")] string WeeklyScheduleFromFile,
[property: CliOption("--start-time")] string StartTime,
[property: CliFlag("--daily-schedule")] bool DailySchedule,
[property: CliOption("--hourly-schedule")] string HourlySchedule,
[property: CliOption("--weekly-schedule")] string WeeklySchedule
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--on-source-disk-delete")]
    public string? OnSourceDiskDelete { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliFlag("--guest-flush")]
    public bool? GuestFlush { get; set; }

    [CliOption("--snapshot-labels")]
    public IEnumerable<KeyValue>? SnapshotLabels { get; set; }

    [CliOption("--storage-location")]
    public string? StorageLocation { get; set; }
}