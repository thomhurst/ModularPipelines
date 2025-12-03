using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "maintenanceconfiguration", "update")]
public record AzAksMaintenanceconfigurationUpdateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--config-file")]
    public string? ConfigFile { get; set; }

    [CliOption("--day-of-month")]
    public string? DayOfMonth { get; set; }

    [CliOption("--day-of-week")]
    public string? DayOfWeek { get; set; }

    [CliOption("--duration")]
    public string? Duration { get; set; }

    [CliOption("--interval-days")]
    public int? IntervalDays { get; set; }

    [CliOption("--interval-months")]
    public int? IntervalMonths { get; set; }

    [CliOption("--interval-weeks")]
    public int? IntervalWeeks { get; set; }

    [CliOption("--schedule-type")]
    public string? ScheduleType { get; set; }

    [CliOption("--start-date")]
    public string? StartDate { get; set; }

    [CliOption("--start-hour")]
    public string? StartHour { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--utc-offset")]
    public string? UtcOffset { get; set; }

    [CliOption("--week-index")]
    public string? WeekIndex { get; set; }

    [CliOption("--weekday")]
    public string? Weekday { get; set; }
}