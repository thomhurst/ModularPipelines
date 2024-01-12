using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routes", "create")]
public record GcloudComputeRoutesCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--destination-range")] string DestinationRange,
[property: CommandSwitch("--next-hop-address")] string NextHopAddress,
[property: CommandSwitch("--next-hop-gateway")] string NextHopGateway,
[property: CommandSwitch("--next-hop-ilb")] string NextHopIlb,
[property: CommandSwitch("--next-hop-instance")] string NextHopInstance,
[property: CommandSwitch("--next-hop-vpn-tunnel")] string NextHopVpnTunnel
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--next-hop-ilb-region")]
    public string? NextHopIlbRegion { get; set; }

    [CommandSwitch("--next-hop-instance-zone")]
    public string? NextHopInstanceZone { get; set; }

    [CommandSwitch("--next-hop-vpn-tunnel-region")]
    public string? NextHopVpnTunnelRegion { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }
}