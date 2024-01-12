using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "nats", "update")]
public record GcloudComputeRoutersNatsUpdateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--router")] string Router
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--auto-network-tier")]
    public string? AutoNetworkTier { get; set; }

    [CommandSwitch("--[no-]enable-dynamic-port-allocation")]
    public string[]? NoEnableDynamicPortAllocation { get; set; }

    [BooleanCommandSwitch("--enable-endpoint-independent-mapping")]
    public bool? EnableEndpointIndependentMapping { get; set; }

    [BooleanCommandSwitch("--enable-logging")]
    public bool? EnableLogging { get; set; }

    [CommandSwitch("--log-filter")]
    public string? LogFilter { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--rules")]
    public string? Rules { get; set; }

    [BooleanCommandSwitch("--auto-allocate-nat-external-ips")]
    public bool? AutoAllocateNatExternalIps { get; set; }

    [CommandSwitch("--nat-external-ip-pool")]
    public string[]? NatExternalIpPool { get; set; }

    [BooleanCommandSwitch("--clear-icmp-idle-timeout")]
    public bool? ClearIcmpIdleTimeout { get; set; }

    [CommandSwitch("--icmp-idle-timeout")]
    public string? IcmpIdleTimeout { get; set; }

    [BooleanCommandSwitch("--clear-max-ports-per-vm")]
    public bool? ClearMaxPortsPerVm { get; set; }

    [CommandSwitch("--max-ports-per-vm")]
    public string? MaxPortsPerVm { get; set; }

    [BooleanCommandSwitch("--clear-min-ports-per-vm")]
    public bool? ClearMinPortsPerVm { get; set; }

    [CommandSwitch("--min-ports-per-vm")]
    public string? MinPortsPerVm { get; set; }

    [BooleanCommandSwitch("--clear-nat-external-drain-ip-pool")]
    public bool? ClearNatExternalDrainIpPool { get; set; }

    [CommandSwitch("--nat-external-drain-ip-pool")]
    public string[]? NatExternalDrainIpPool { get; set; }

    [BooleanCommandSwitch("--clear-tcp-established-idle-timeout")]
    public bool? ClearTcpEstablishedIdleTimeout { get; set; }

    [CommandSwitch("--tcp-established-idle-timeout")]
    public string? TcpEstablishedIdleTimeout { get; set; }

    [BooleanCommandSwitch("--clear-tcp-time-wait-timeout")]
    public bool? ClearTcpTimeWaitTimeout { get; set; }

    [CommandSwitch("--tcp-time-wait-timeout")]
    public string? TcpTimeWaitTimeout { get; set; }

    [BooleanCommandSwitch("--clear-tcp-transitory-idle-timeout")]
    public bool? ClearTcpTransitoryIdleTimeout { get; set; }

    [CommandSwitch("--tcp-transitory-idle-timeout")]
    public string? TcpTransitoryIdleTimeout { get; set; }

    [BooleanCommandSwitch("--clear-udp-idle-timeout")]
    public bool? ClearUdpIdleTimeout { get; set; }

    [CommandSwitch("--udp-idle-timeout")]
    public string? UdpIdleTimeout { get; set; }

    [BooleanCommandSwitch("--nat-all-subnet-ip-ranges")]
    public bool? NatAllSubnetIpRanges { get; set; }

    [CommandSwitch("--nat-custom-subnet-ip-ranges")]
    public string[]? NatCustomSubnetIpRanges { get; set; }

    [BooleanCommandSwitch("--nat-primary-subnet-ip-ranges")]
    public bool? NatPrimarySubnetIpRanges { get; set; }
}