using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "connection-monitor", "endpoint", "show")]
public record AzNetworkWatcherConnectionMonitorEndpointShowOptions(
[property: CommandSwitch("--connection-monitor")] string ConnectionMonitor,
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--location")] string Location
) : AzOptions;