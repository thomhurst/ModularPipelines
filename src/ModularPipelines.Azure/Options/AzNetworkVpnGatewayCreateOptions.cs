using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-gateway", "create")]
public record AzNetworkVpnGatewayCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--asn")]
    public string? Asn { get; set; }

    [CommandSwitch("--bgp-peering-address")]
    public string? BgpPeeringAddress { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--peer-weight")]
    public string? PeerWeight { get; set; }

    [CommandSwitch("--scale-unit")]
    public string? ScaleUnit { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vhub")]
    public string? Vhub { get; set; }
}