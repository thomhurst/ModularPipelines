using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "connection-monitor", "test-group", "add")]
public record AzNetworkWatcherConnectionMonitorTestGroupAddOptions(
[property: CommandSwitch("--connection-monitor")] string ConnectionMonitor,
[property: CommandSwitch("--endpoint-dest-name")] string EndpointDestName,
[property: CommandSwitch("--endpoint-source-name")] string EndpointSourceName,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--test-config-name")] string TestConfigName
) : AzOptions
{
    [BooleanCommandSwitch("--disable")]
    public bool? Disable { get; set; }

    [CommandSwitch("--endpoint-dest-address")]
    public string? EndpointDestAddress { get; set; }

    [CommandSwitch("--endpoint-dest-resource-id")]
    public string? EndpointDestResourceId { get; set; }

    [CommandSwitch("--endpoint-source-address")]
    public string? EndpointSourceAddress { get; set; }

    [CommandSwitch("--endpoint-source-resource-id")]
    public string? EndpointSourceResourceId { get; set; }

    [CommandSwitch("--frequency")]
    public string? Frequency { get; set; }

    [CommandSwitch("--http-method")]
    public string? HttpMethod { get; set; }

    [CommandSwitch("--http-path")]
    public string? HttpPath { get; set; }

    [CommandSwitch("--http-port")]
    public string? HttpPort { get; set; }

    [CommandSwitch("--http-valid-status-codes")]
    public string? HttpValidStatusCodes { get; set; }

    [BooleanCommandSwitch("--https-prefer")]
    public bool? HttpsPrefer { get; set; }

    [BooleanCommandSwitch("--icmp-disable-trace-route")]
    public bool? IcmpDisableTraceRoute { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--preferred-ip-version")]
    public string? PreferredIpVersion { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [BooleanCommandSwitch("--tcp-disable-trace-route")]
    public bool? TcpDisableTraceRoute { get; set; }

    [CommandSwitch("--tcp-port")]
    public string? TcpPort { get; set; }

    [CommandSwitch("--threshold-failed-percent")]
    public string? ThresholdFailedPercent { get; set; }

    [CommandSwitch("--threshold-round-trip-time")]
    public string? ThresholdRoundTripTime { get; set; }
}