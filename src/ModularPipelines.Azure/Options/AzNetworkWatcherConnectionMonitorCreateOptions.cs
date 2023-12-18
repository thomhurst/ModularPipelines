using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "connection-monitor", "create")]
public record AzNetworkWatcherConnectionMonitorCreateOptions(
[property: CommandSwitch("--connection-monitor-name")] string ConnectionMonitorName,
[property: CommandSwitch("--endpoint-dest-name")] string EndpointDestName,
[property: CommandSwitch("--endpoint-source-name")] string EndpointSourceName,
[property: CommandSwitch("--endpoint-source-resource-id")] string EndpointSourceResourceId,
[property: CommandSwitch("--test-config-name")] string TestConfigName
) : AzOptions
{
    [CommandSwitch("--endpoint-dest-address")]
    public string? EndpointDestAddress { get; set; }

    [CommandSwitch("--endpoint-dest-coverage-level")]
    public string? EndpointDestCoverageLevel { get; set; }

    [CommandSwitch("--endpoint-dest-resource-id")]
    public string? EndpointDestResourceId { get; set; }

    [CommandSwitch("--endpoint-dest-type")]
    public string? EndpointDestType { get; set; }

    [CommandSwitch("--endpoint-source-address")]
    public string? EndpointSourceAddress { get; set; }

    [CommandSwitch("--endpoint-source-coverage-level")]
    public string? EndpointSourceCoverageLevel { get; set; }

    [CommandSwitch("--endpoint-source-type")]
    public string? EndpointSourceType { get; set; }

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

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--notes")]
    public string? Notes { get; set; }

    [CommandSwitch("--output-type")]
    public string? OutputType { get; set; }

    [CommandSwitch("--preferred-ip-version")]
    public string? PreferredIpVersion { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--tcp-disable-trace-route")]
    public bool? TcpDisableTraceRoute { get; set; }

    [CommandSwitch("--tcp-port")]
    public string? TcpPort { get; set; }

    [CommandSwitch("--tcp-port-behavior")]
    public string? TcpPortBehavior { get; set; }

    [BooleanCommandSwitch("--test-group-disable")]
    public bool? TestGroupDisable { get; set; }

    [CommandSwitch("--test-group-name")]
    public string? TestGroupName { get; set; }

    [CommandSwitch("--threshold-failed-percent")]
    public string? ThresholdFailedPercent { get; set; }

    [CommandSwitch("--threshold-round-trip-time")]
    public string? ThresholdRoundTripTime { get; set; }

    [CommandSwitch("--workspace-ids")]
    public string? WorkspaceIds { get; set; }
}