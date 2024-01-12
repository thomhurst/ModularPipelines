using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "nats", "create")]
public record GcloudComputeRoutersNatsCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--router")] string Router,
[property: BooleanCommandSwitch("--nat-all-subnet-ip-ranges")] bool NatAllSubnetIpRanges,
[property: CommandSwitch("--nat-custom-subnet-ip-ranges")] string[] NatCustomSubnetIpRanges,
[property: BooleanCommandSwitch("--nat-primary-subnet-ip-ranges")] bool NatPrimarySubnetIpRanges
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

    [CommandSwitch("--endpoint-types")]
    public string[]? EndpointTypes { get; set; }

    [CommandSwitch("--icmp-idle-timeout")]
    public string? IcmpIdleTimeout { get; set; }

    [CommandSwitch("--log-filter")]
    public string? LogFilter { get; set; }

    [CommandSwitch("--max-ports-per-vm")]
    public string? MaxPortsPerVm { get; set; }

    [CommandSwitch("--min-ports-per-vm")]
    public string? MinPortsPerVm { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--rules")]
    public string? Rules { get; set; }

    [CommandSwitch("--tcp-established-idle-timeout")]
    public string? TcpEstablishedIdleTimeout { get; set; }

    [CommandSwitch("--tcp-time-wait-timeout")]
    public string? TcpTimeWaitTimeout { get; set; }

    [CommandSwitch("--tcp-transitory-idle-timeout")]
    public string? TcpTransitoryIdleTimeout { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--udp-idle-timeout")]
    public string? UdpIdleTimeout { get; set; }

    [BooleanCommandSwitch("--auto-allocate-nat-external-ips")]
    public bool? AutoAllocateNatExternalIps { get; set; }

    [CommandSwitch("--nat-external-ip-pool")]
    public string[]? NatExternalIpPool { get; set; }
}