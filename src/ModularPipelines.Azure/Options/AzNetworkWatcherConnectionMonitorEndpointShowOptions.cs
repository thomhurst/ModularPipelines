using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "watcher", "connection-monitor", "endpoint", "show")]
public record AzNetworkWatcherConnectionMonitorEndpointShowOptions(
[property: CliOption("--connection-monitor")] string ConnectionMonitor,
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--location")] string Location
) : AzOptions;