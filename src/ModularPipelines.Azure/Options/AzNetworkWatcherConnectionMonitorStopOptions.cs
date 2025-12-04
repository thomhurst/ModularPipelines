using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "connection-monitor", "stop")]
public record AzNetworkWatcherConnectionMonitorStopOptions(
[property: CliOption("--connection-monitor-name")] string ConnectionMonitorName,
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}