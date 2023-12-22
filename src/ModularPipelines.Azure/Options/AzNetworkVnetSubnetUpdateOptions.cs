using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet", "subnet", "update")]
public record AzNetworkVnetSubnetUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

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

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--nat-gateway")]
    public string? NatGateway { get; set; }

    [CommandSwitch("--network-security-group")]
    public string? NetworkSecurityGroup { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--route-table")]
    public string? RouteTable { get; set; }

    [CommandSwitch("--service-endpoint-policy")]
    public string? ServiceEndpointPolicy { get; set; }

    [CommandSwitch("--service-endpoints")]
    public string? ServiceEndpoints { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}