using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routes", "create")]
public record GcloudComputeRoutesCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--destination-range")] string DestinationRange,
[property: CliOption("--next-hop-address")] string NextHopAddress,
[property: CliOption("--next-hop-gateway")] string NextHopGateway,
[property: CliOption("--next-hop-ilb")] string NextHopIlb,
[property: CliOption("--next-hop-instance")] string NextHopInstance,
[property: CliOption("--next-hop-vpn-tunnel")] string NextHopVpnTunnel
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--next-hop-ilb-region")]
    public string? NextHopIlbRegion { get; set; }

    [CliOption("--next-hop-instance-zone")]
    public string? NextHopInstanceZone { get; set; }

    [CliOption("--next-hop-vpn-tunnel-region")]
    public string? NextHopVpnTunnelRegion { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }
}