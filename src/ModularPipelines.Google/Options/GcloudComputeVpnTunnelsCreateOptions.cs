using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "vpn-tunnels", "create")]
public record GcloudComputeVpnTunnelsCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--shared-secret")] string SharedSecret,
[property: CommandSwitch("--peer-address")] string PeerAddress,
[property: CommandSwitch("--peer-external-gateway")] string PeerExternalGateway,
[property: CommandSwitch("--peer-gcp-gateway")] string PeerGcpGateway,
[property: CommandSwitch("--peer-gcp-gateway-region")] string PeerGcpGatewayRegion,
[property: CommandSwitch("--target-vpn-gateway")] string TargetVpnGateway,
[property: CommandSwitch("--target-vpn-gateway-region")] string TargetVpnGatewayRegion,
[property: CommandSwitch("--vpn-gateway")] string VpnGateway,
[property: CommandSwitch("--vpn-gateway-region")] string VpnGatewayRegion
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--ike-version")]
    public string? IkeVersion { get; set; }

    [CommandSwitch("--interface")]
    public string? Interface { get; set; }

    [CommandSwitch("--local-traffic-selector")]
    public string[]? LocalTrafficSelector { get; set; }

    [CommandSwitch("--peer-external-gateway-interface")]
    public string? PeerExternalGatewayInterface { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--remote-traffic-selector")]
    public string[]? RemoteTrafficSelector { get; set; }

    [CommandSwitch("--router")]
    public string? Router { get; set; }

    [CommandSwitch("--router-region")]
    public string? RouterRegion { get; set; }
}