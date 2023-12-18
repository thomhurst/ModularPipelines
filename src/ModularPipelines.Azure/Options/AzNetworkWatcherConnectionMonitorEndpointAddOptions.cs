using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "connection-monitor", "endpoint", "add")]
public record AzNetworkWatcherConnectionMonitorEndpointAddOptions(
[property: CommandSwitch("--connection-monitor")] string ConnectionMonitor,
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [CommandSwitch("--address-exclude")]
    public string? AddressExclude { get; set; }

    [CommandSwitch("--address-include")]
    public string? AddressInclude { get; set; }

    [CommandSwitch("--coverage-level")]
    public string? CoverageLevel { get; set; }

    [CommandSwitch("--dest-test-groups")]
    public string? DestTestGroups { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }

    [CommandSwitch("--source-test-groups")]
    public string? SourceTestGroups { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}