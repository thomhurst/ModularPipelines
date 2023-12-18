using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet", "peering", "create")]
public record AzNetworkVnetPeeringCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--remote-vnet")] string RemoteVnet,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vnet-name")] string VnetName
) : AzOptions
{
    [BooleanCommandSwitch("--allow-forwarded-traffic")]
    public bool? AllowForwardedTraffic { get; set; }

    [BooleanCommandSwitch("--allow-gateway-transit")]
    public bool? AllowGatewayTransit { get; set; }

    [BooleanCommandSwitch("--allow-vnet-access")]
    public bool? AllowVnetAccess { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--use-remote-gateways")]
    public bool? UseRemoteGateways { get; set; }
}