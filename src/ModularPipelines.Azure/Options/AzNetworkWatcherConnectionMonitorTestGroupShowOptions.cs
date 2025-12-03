using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "watcher", "connection-monitor", "test-group", "show")]
public record AzNetworkWatcherConnectionMonitorTestGroupShowOptions(
[property: CliOption("--connection-monitor")] string ConnectionMonitor,
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name
) : AzOptions;