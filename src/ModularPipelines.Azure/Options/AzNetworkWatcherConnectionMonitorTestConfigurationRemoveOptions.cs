using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "watcher", "connection-monitor", "test-configuration", "remove")]
public record AzNetworkWatcherConnectionMonitorTestConfigurationRemoveOptions(
[property: CliOption("--connection-monitor")] string ConnectionMonitor,
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--test-groups")]
    public string? TestGroups { get; set; }
}