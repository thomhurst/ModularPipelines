using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "connection-monitor", "test-group", "list")]
public record AzNetworkWatcherConnectionMonitorTestGroupListOptions(
[property: CliOption("--connection-monitor")] string ConnectionMonitor,
[property: CliOption("--location")] string Location
) : AzOptions;