using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memcache", "instances", "create")]
public record GcloudMemcacheInstancesCreateOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region,
[property: CliOption("--node-count")] string NodeCount,
[property: CliOption("--node-cpu")] string NodeCpu,
[property: CliOption("--node-memory")] string NodeMemory
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--authorized-network")]
    public string? AuthorizedNetwork { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CliOption("--maintenance-window-duration")]
    public string? MaintenanceWindowDuration { get; set; }

    [CliOption("--maintenance-window-start-time")]
    public string? MaintenanceWindowStartTime { get; set; }

    [CliOption("--memcached-version")]
    public string? MemcachedVersion { get; set; }

    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--reserved-ip-range-id")]
    public string[]? ReservedIpRangeId { get; set; }

    [CliOption("--zones")]
    public string[]? Zones { get; set; }
}