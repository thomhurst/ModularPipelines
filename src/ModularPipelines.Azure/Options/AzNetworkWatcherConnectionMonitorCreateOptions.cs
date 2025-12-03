using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "watcher", "connection-monitor", "create")]
public record AzNetworkWatcherConnectionMonitorCreateOptions(
[property: CliOption("--connection-monitor-name")] string ConnectionMonitorName,
[property: CliOption("--endpoint-dest-name")] string EndpointDestName,
[property: CliOption("--endpoint-source-name")] string EndpointSourceName,
[property: CliOption("--endpoint-source-resource-id")] string EndpointSourceResourceId,
[property: CliOption("--test-config-name")] string TestConfigName
) : AzOptions
{
    [CliOption("--endpoint-dest-address")]
    public string? EndpointDestAddress { get; set; }

    [CliOption("--endpoint-dest-coverage-level")]
    public string? EndpointDestCoverageLevel { get; set; }

    [CliOption("--endpoint-dest-resource-id")]
    public string? EndpointDestResourceId { get; set; }

    [CliOption("--endpoint-dest-type")]
    public string? EndpointDestType { get; set; }

    [CliOption("--endpoint-source-address")]
    public string? EndpointSourceAddress { get; set; }

    [CliOption("--endpoint-source-coverage-level")]
    public string? EndpointSourceCoverageLevel { get; set; }

    [CliOption("--endpoint-source-type")]
    public string? EndpointSourceType { get; set; }

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

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--notes")]
    public string? Notes { get; set; }

    [CliOption("--output-type")]
    public string? OutputType { get; set; }

    [CliOption("--preferred-ip-version")]
    public string? PreferredIpVersion { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--tcp-disable-trace-route")]
    public bool? TcpDisableTraceRoute { get; set; }

    [CliOption("--tcp-port")]
    public string? TcpPort { get; set; }

    [CliOption("--tcp-port-behavior")]
    public string? TcpPortBehavior { get; set; }

    [CliFlag("--test-group-disable")]
    public bool? TestGroupDisable { get; set; }

    [CliOption("--test-group-name")]
    public string? TestGroupName { get; set; }

    [CliOption("--threshold-failed-percent")]
    public string? ThresholdFailedPercent { get; set; }

    [CliOption("--threshold-round-trip-time")]
    public string? ThresholdRoundTripTime { get; set; }

    [CliOption("--workspace-ids")]
    public string? WorkspaceIds { get; set; }
}