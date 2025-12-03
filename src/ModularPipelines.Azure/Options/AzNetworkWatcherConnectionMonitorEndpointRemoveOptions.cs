using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "watcher", "connection-monitor", "endpoint", "remove")]
public record AzNetworkWatcherConnectionMonitorEndpointRemoveOptions(
[property: CliOption("--connection-monitor")] string ConnectionMonitor,
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--test-groups")]
    public string? TestGroups { get; set; }
}