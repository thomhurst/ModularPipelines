using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "connection-monitor", "show")]
public record AzNetworkWatcherConnectionMonitorShowOptions(
[property: CommandSwitch("--connection-monitor-name")] string ConnectionMonitorName,
[property: CommandSwitch("--location")] string Location
) : AzOptions;