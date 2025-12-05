using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vnet", "peering", "create")]
public record AzNetworkVnetPeeringCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--remote-vnet")] string RemoteVnet,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vnet-name")] string VnetName
) : AzOptions
{
    [CliFlag("--allow-forwarded-traffic")]
    public bool? AllowForwardedTraffic { get; set; }

    [CliFlag("--allow-gateway-transit")]
    public bool? AllowGatewayTransit { get; set; }

    [CliFlag("--allow-vnet-access")]
    public bool? AllowVnetAccess { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--use-remote-gateways")]
    public bool? UseRemoteGateways { get; set; }
}