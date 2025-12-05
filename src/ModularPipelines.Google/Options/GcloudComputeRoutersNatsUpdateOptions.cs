using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "nats", "update")]
public record GcloudComputeRoutersNatsUpdateOptions(
[property: CliArgument] string Name,
[property: CliOption("--router")] string Router
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--auto-network-tier")]
    public string? AutoNetworkTier { get; set; }

    [CliOption("--[no-]enable-dynamic-port-allocation")]
    public string[]? NoEnableDynamicPortAllocation { get; set; }

    [CliFlag("--enable-endpoint-independent-mapping")]
    public bool? EnableEndpointIndependentMapping { get; set; }

    [CliFlag("--enable-logging")]
    public bool? EnableLogging { get; set; }

    [CliOption("--log-filter")]
    public string? LogFilter { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--rules")]
    public string? Rules { get; set; }

    [CliFlag("--auto-allocate-nat-external-ips")]
    public bool? AutoAllocateNatExternalIps { get; set; }

    [CliOption("--nat-external-ip-pool")]
    public string[]? NatExternalIpPool { get; set; }

    [CliFlag("--clear-icmp-idle-timeout")]
    public bool? ClearIcmpIdleTimeout { get; set; }

    [CliOption("--icmp-idle-timeout")]
    public string? IcmpIdleTimeout { get; set; }

    [CliFlag("--clear-max-ports-per-vm")]
    public bool? ClearMaxPortsPerVm { get; set; }

    [CliOption("--max-ports-per-vm")]
    public string? MaxPortsPerVm { get; set; }

    [CliFlag("--clear-min-ports-per-vm")]
    public bool? ClearMinPortsPerVm { get; set; }

    [CliOption("--min-ports-per-vm")]
    public string? MinPortsPerVm { get; set; }

    [CliFlag("--clear-nat-external-drain-ip-pool")]
    public bool? ClearNatExternalDrainIpPool { get; set; }

    [CliOption("--nat-external-drain-ip-pool")]
    public string[]? NatExternalDrainIpPool { get; set; }

    [CliFlag("--clear-tcp-established-idle-timeout")]
    public bool? ClearTcpEstablishedIdleTimeout { get; set; }

    [CliOption("--tcp-established-idle-timeout")]
    public string? TcpEstablishedIdleTimeout { get; set; }

    [CliFlag("--clear-tcp-time-wait-timeout")]
    public bool? ClearTcpTimeWaitTimeout { get; set; }

    [CliOption("--tcp-time-wait-timeout")]
    public string? TcpTimeWaitTimeout { get; set; }

    [CliFlag("--clear-tcp-transitory-idle-timeout")]
    public bool? ClearTcpTransitoryIdleTimeout { get; set; }

    [CliOption("--tcp-transitory-idle-timeout")]
    public string? TcpTransitoryIdleTimeout { get; set; }

    [CliFlag("--clear-udp-idle-timeout")]
    public bool? ClearUdpIdleTimeout { get; set; }

    [CliOption("--udp-idle-timeout")]
    public string? UdpIdleTimeout { get; set; }

    [CliFlag("--nat-all-subnet-ip-ranges")]
    public bool? NatAllSubnetIpRanges { get; set; }

    [CliOption("--nat-custom-subnet-ip-ranges")]
    public string[]? NatCustomSubnetIpRanges { get; set; }

    [CliFlag("--nat-primary-subnet-ip-ranges")]
    public bool? NatPrimarySubnetIpRanges { get; set; }
}