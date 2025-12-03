using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "watcher", "connection-monitor", "test-group", "add")]
public record AzNetworkWatcherConnectionMonitorTestGroupAddOptions(
[property: CliOption("--connection-monitor")] string ConnectionMonitor,
[property: CliOption("--endpoint-dest-name")] string EndpointDestName,
[property: CliOption("--endpoint-source-name")] string EndpointSourceName,
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--test-config-name")] string TestConfigName
) : AzOptions
{
    [CliFlag("--disable")]
    public bool? Disable { get; set; }

    [CliOption("--endpoint-dest-address")]
    public string? EndpointDestAddress { get; set; }

    [CliOption("--endpoint-dest-resource-id")]
    public string? EndpointDestResourceId { get; set; }

    [CliOption("--endpoint-source-address")]
    public string? EndpointSourceAddress { get; set; }

    [CliOption("--endpoint-source-resource-id")]
    public string? EndpointSourceResourceId { get; set; }

    [CliOption("--frequency")]
    public string? Frequency { get; set; }

    [CliOption("--http-method")]
    public string? HttpMethod { get; set; }

    [CliOption("--http-path")]
    public string? HttpPath { get; set; }

    [CliOption("--http-port")]
    public string? HttpPort { get; set; }

    [CliOption("--http-valid-status-codes")]
    public string? HttpValidStatusCodes { get; set; }

    [CliFlag("--https-prefer")]
    public bool? HttpsPrefer { get; set; }

    [CliFlag("--icmp-disable-trace-route")]
    public bool? IcmpDisableTraceRoute { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--preferred-ip-version")]
    public string? PreferredIpVersion { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliFlag("--tcp-disable-trace-route")]
    public bool? TcpDisableTraceRoute { get; set; }

    [CliOption("--tcp-port")]
    public string? TcpPort { get; set; }

    [CliOption("--threshold-failed-percent")]
    public string? ThresholdFailedPercent { get; set; }

    [CliOption("--threshold-round-trip-time")]
    public string? ThresholdRoundTripTime { get; set; }
}