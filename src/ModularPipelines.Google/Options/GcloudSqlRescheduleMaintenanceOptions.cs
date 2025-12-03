using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "reschedule-maintenance")]
public record GcloudSqlRescheduleMaintenanceOptions(
[property: CliArgument] string Instance,
[property: CliOption("--reschedule-type")] string RescheduleType
) : GcloudOptions
{
    [CliOption("--schedule-time")]
    public string? ScheduleTime { get; set; }
}