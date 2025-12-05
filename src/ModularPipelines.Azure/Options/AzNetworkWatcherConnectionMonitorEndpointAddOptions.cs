using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "connection-monitor", "endpoint", "add")]
public record AzNetworkWatcherConnectionMonitorEndpointAddOptions(
[property: CliOption("--connection-monitor")] string ConnectionMonitor,
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliOption("--address")]
    public string? Address { get; set; }

    [CliOption("--address-exclude")]
    public string? AddressExclude { get; set; }

    [CliOption("--address-include")]
    public string? AddressInclude { get; set; }

    [CliOption("--coverage-level")]
    public string? CoverageLevel { get; set; }

    [CliOption("--dest-test-groups")]
    public string? DestTestGroups { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }

    [CliOption("--source-test-groups")]
    public string? SourceTestGroups { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}