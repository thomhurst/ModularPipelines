using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "instances", "reschedule-maintenance")]
public record GcloudRedisInstancesRescheduleMaintenanceOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region,
[property: CliOption("--reschedule-type")] string RescheduleType
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--schedule-time")]
    public string? ScheduleTime { get; set; }
}