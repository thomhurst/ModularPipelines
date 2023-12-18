using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "connection-monitor", "test-configuration", "add")]
public record AzNetworkWatcherConnectionMonitorTestConfigurationAddOptions(
[property: CommandSwitch("--connection-monitor")] string ConnectionMonitor,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--test-groups")] string TestGroups
) : AzOptions
{
    [CommandSwitch("--frequency")]
    public string? Frequency { get; set; }

    [CommandSwitch("--http-method")]
    public string? HttpMethod { get; set; }

    [CommandSwitch("--http-path")]
    public string? HttpPath { get; set; }

    [CommandSwitch("--http-port")]
    public string? HttpPort { get; set; }

    [BooleanCommandSwitch("--http-prefer-https")]
    public bool? HttpPreferHttps { get; set; }

    [CommandSwitch("--http-request-header")]
    public string? HttpRequestHeader { get; set; }

    [CommandSwitch("--http-valid-status-codes")]
    public string? HttpValidStatusCodes { get; set; }

    [BooleanCommandSwitch("--icmp-disable-trace-route")]
    public bool? IcmpDisableTraceRoute { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--preferred-ip-version")]
    public string? PreferredIpVersion { get; set; }

    [BooleanCommandSwitch("--tcp-disable-trace-route")]
    public bool? TcpDisableTraceRoute { get; set; }

    [CommandSwitch("--tcp-port")]
    public string? TcpPort { get; set; }

    [CommandSwitch("--tcp-port-behavior")]
    public string? TcpPortBehavior { get; set; }

    [CommandSwitch("--threshold-failed-percent")]
    public string? ThresholdFailedPercent { get; set; }

    [CommandSwitch("--threshold-round-trip-time")]
    public string? ThresholdRoundTripTime { get; set; }
}