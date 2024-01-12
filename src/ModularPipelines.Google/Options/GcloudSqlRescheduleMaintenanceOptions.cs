using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "reschedule-maintenance")]
public record GcloudSqlRescheduleMaintenanceOptions(
[property: PositionalArgument] string Instance,
[property: CommandSwitch("--reschedule-type")] string RescheduleType
) : GcloudOptions
{
    [CommandSwitch("--schedule-time")]
    public string? ScheduleTime { get; set; }
}