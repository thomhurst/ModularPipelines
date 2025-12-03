using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "resource-policies", "update", "snapshot-schedule")]
public record GcloudComputeResourcePoliciesUpdateSnapshotScheduleOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--max-retention-days")]
    public string? MaxRetentionDays { get; set; }

    [CliOption("--on-source-disk-delete")]
    public string? OnSourceDiskDelete { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--snapshot-labels")]
    public IEnumerable<KeyValue>? SnapshotLabels { get; set; }

    [CliOption("--weekly-schedule-from-file")]
    public string? WeeklyScheduleFromFile { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliFlag("--daily-schedule")]
    public bool? DailySchedule { get; set; }

    [CliOption("--hourly-schedule")]
    public string? HourlySchedule { get; set; }

    [CliOption("--weekly-schedule")]
    public string? WeeklySchedule { get; set; }
}