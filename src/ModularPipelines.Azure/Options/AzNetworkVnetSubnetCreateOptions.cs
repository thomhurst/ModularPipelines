using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet", "subnet", "create")]
public record AzNetworkVnetSubnetCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vnet-name")] string VnetName
) : AzOptions
{
    [CommandSwitch("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [BooleanCommandSwitch("--default-outbound")]
    public bool? DefaultOutbound { get; set; }

    [CommandSwitch("--delegations")]
    public string? Delegations { get; set; }

    [BooleanCommandSwitch("--disable-private-endpoint-network-policies")]
    public bool? DisablePrivateEndpointNetworkPolicies { get; set; }

    [BooleanCommandSwitch("--disable-private-link-service-network-policies")]
    public bool? DisablePrivateLinkServiceNetworkPolicies { get; set; }

    [CommandSwitch("--nat-gateway")]
    public string? NatGateway { get; set; }

    [CommandSwitch("--network-security-group")]
    public string? NetworkSecurityGroup { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--route-table")]
    public string? RouteTable { get; set; }

    [CommandSwitch("--service-endpoint-policy")]
    public string? ServiceEndpointPolicy { get; set; }

    [CommandSwitch("--service-endpoints")]
    public string? ServiceEndpoints { get; set; }
}