using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "nats", "create")]
public record GcloudComputeRoutersNatsCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--router")] string Router,
[property: CliFlag("--nat-all-subnet-ip-ranges")] bool NatAllSubnetIpRanges,
[property: CliOption("--nat-custom-subnet-ip-ranges")] string[] NatCustomSubnetIpRanges,
[property: CliFlag("--nat-primary-subnet-ip-ranges")] bool NatPrimarySubnetIpRanges
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

    [CliOption("--endpoint-types")]
    public string[]? EndpointTypes { get; set; }

    [CliOption("--icmp-idle-timeout")]
    public string? IcmpIdleTimeout { get; set; }

    [CliOption("--log-filter")]
    public string? LogFilter { get; set; }

    [CliOption("--max-ports-per-vm")]
    public string? MaxPortsPerVm { get; set; }

    [CliOption("--min-ports-per-vm")]
    public string? MinPortsPerVm { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--rules")]
    public string? Rules { get; set; }

    [CliOption("--tcp-established-idle-timeout")]
    public string? TcpEstablishedIdleTimeout { get; set; }

    [CliOption("--tcp-time-wait-timeout")]
    public string? TcpTimeWaitTimeout { get; set; }

    [CliOption("--tcp-transitory-idle-timeout")]
    public string? TcpTransitoryIdleTimeout { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--udp-idle-timeout")]
    public string? UdpIdleTimeout { get; set; }

    [CliFlag("--auto-allocate-nat-external-ips")]
    public bool? AutoAllocateNatExternalIps { get; set; }

    [CliOption("--nat-external-ip-pool")]
    public string[]? NatExternalIpPool { get; set; }
}