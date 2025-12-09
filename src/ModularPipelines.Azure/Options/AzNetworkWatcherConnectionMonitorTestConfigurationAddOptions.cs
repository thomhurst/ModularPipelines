using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "connection-monitor", "test-configuration", "add")]
public record AzNetworkWatcherConnectionMonitorTestConfigurationAddOptions(
[property: CliOption("--connection-monitor")] string ConnectionMonitor,
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--protocol")] string Protocol,
[property: CliOption("--test-groups")] string TestGroups
) : AzOptions
{
    [CliOption("--frequency")]
    public string? Frequency { get; set; }

    [CliOption("--http-method")]
    public string? HttpMethod { get; set; }

    [CliOption("--http-path")]
    public string? HttpPath { get; set; }

    [CliOption("--http-port")]
    public string? HttpPort { get; set; }

    [CliFlag("--http-prefer-https")]
    public bool? HttpPreferHttps { get; set; }

    [CliOption("--http-request-header")]
    public string? HttpRequestHeader { get; set; }

    [CliOption("--http-valid-status-codes")]
    public string? HttpValidStatusCodes { get; set; }

    [CliFlag("--icmp-disable-trace-route")]
    public bool? IcmpDisableTraceRoute { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--preferred-ip-version")]
    public string? PreferredIpVersion { get; set; }

    [CliFlag("--tcp-disable-trace-route")]
    public bool? TcpDisableTraceRoute { get; set; }

    [CliOption("--tcp-port")]
    public string? TcpPort { get; set; }

    [CliOption("--tcp-port-behavior")]
    public string? TcpPortBehavior { get; set; }

    [CliOption("--threshold-failed-percent")]
    public string? ThresholdFailedPercent { get; set; }

    [CliOption("--threshold-round-trip-time")]
    public string? ThresholdRoundTripTime { get; set; }
}