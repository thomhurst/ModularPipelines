using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "connection-monitor", "show")]
public record AzNetworkWatcherConnectionMonitorShowOptions(
[property: CliOption("--connection-monitor-name")] string ConnectionMonitorName,
[property: CliOption("--location")] string Location
) : AzOptions;