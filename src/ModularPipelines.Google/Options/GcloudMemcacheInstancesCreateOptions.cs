using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memcache", "instances", "create")]
public record GcloudMemcacheInstancesCreateOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--node-count")] string NodeCount,
[property: CommandSwitch("--node-cpu")] string NodeCpu,
[property: CommandSwitch("--node-memory")] string NodeMemory
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--authorized-network")]
    public string? AuthorizedNetwork { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CommandSwitch("--maintenance-window-duration")]
    public string? MaintenanceWindowDuration { get; set; }

    [CommandSwitch("--maintenance-window-start-time")]
    public string? MaintenanceWindowStartTime { get; set; }

    [CommandSwitch("--memcached-version")]
    public string? MemcachedVersion { get; set; }

    [CommandSwitch("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CommandSwitch("--reserved-ip-range-id")]
    public string[]? ReservedIpRangeId { get; set; }

    [CommandSwitch("--zones")]
    public string[]? Zones { get; set; }
}