using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "vpn-tunnels", "create")]
public record GcloudComputeVpnTunnelsCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--shared-secret")] string SharedSecret,
[property: CliOption("--peer-address")] string PeerAddress,
[property: CliOption("--peer-external-gateway")] string PeerExternalGateway,
[property: CliOption("--peer-gcp-gateway")] string PeerGcpGateway,
[property: CliOption("--peer-gcp-gateway-region")] string PeerGcpGatewayRegion,
[property: CliOption("--target-vpn-gateway")] string TargetVpnGateway,
[property: CliOption("--target-vpn-gateway-region")] string TargetVpnGatewayRegion,
[property: CliOption("--vpn-gateway")] string VpnGateway,
[property: CliOption("--vpn-gateway-region")] string VpnGatewayRegion
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--ike-version")]
    public string? IkeVersion { get; set; }

    [CliOption("--interface")]
    public string? Interface { get; set; }

    [CliOption("--local-traffic-selector")]
    public string[]? LocalTrafficSelector { get; set; }

    [CliOption("--peer-external-gateway-interface")]
    public string? PeerExternalGatewayInterface { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--remote-traffic-selector")]
    public string[]? RemoteTrafficSelector { get; set; }

    [CliOption("--router")]
    public string? Router { get; set; }

    [CliOption("--router-region")]
    public string? RouterRegion { get; set; }
}