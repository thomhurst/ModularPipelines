using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "watcher", "connection-monitor", "list")]
public record AzNetworkWatcherConnectionMonitorListOptions(
[property: CliOption("--location")] string Location
) : AzOptions;