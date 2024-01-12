using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memcache", "instances", "reschedule-maintenance")]
public record GcloudMemcacheInstancesRescheduleMaintenanceOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--reschedule-type")] string RescheduleType
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--schedule-time")]
    public string? ScheduleTime { get; set; }
}