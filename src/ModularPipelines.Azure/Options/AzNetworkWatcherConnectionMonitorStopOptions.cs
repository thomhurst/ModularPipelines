using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "connection-monitor", "stop")]
public record AzNetworkWatcherConnectionMonitorStopOptions(
[property: CommandSwitch("--connection-monitor-name")] string ConnectionMonitorName,
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}