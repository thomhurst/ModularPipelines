using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "connection-monitor", "output", "remove")]
public record AzNetworkWatcherConnectionMonitorOutputRemoveOptions(
[property: CommandSwitch("--connection-monitor")] string ConnectionMonitor,
[property: CommandSwitch("--location")] string Location
) : AzOptions;