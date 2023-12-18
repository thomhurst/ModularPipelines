using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "maintenanceconfiguration", "add", "(aks-preview", "extension)")]
public record AzAksMaintenanceconfigurationAddAksPreviewExtensionOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--config-file")]
    public string? ConfigFile { get; set; }

    [CommandSwitch("--day-of-month")]
    public string? DayOfMonth { get; set; }

    [CommandSwitch("--day-of-week")]
    public string? DayOfWeek { get; set; }

    [CommandSwitch("--duration")]
    public string? Duration { get; set; }

    [CommandSwitch("--interval-days")]
    public int? IntervalDays { get; set; }

    [CommandSwitch("--interval-months")]
    public int? IntervalMonths { get; set; }

    [CommandSwitch("--interval-weeks")]
    public int? IntervalWeeks { get; set; }

    [CommandSwitch("--schedule-type")]
    public string? ScheduleType { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }

    [CommandSwitch("--start-hour")]
    public string? StartHour { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--utc-offset")]
    public string? UtcOffset { get; set; }

    [CommandSwitch("--week-index")]
    public string? WeekIndex { get; set; }

    [CommandSwitch("--weekday")]
    public string? Weekday { get; set; }
}

