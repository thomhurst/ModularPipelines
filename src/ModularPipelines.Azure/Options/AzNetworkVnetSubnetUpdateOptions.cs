using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vnet", "subnet", "update")]
public record AzNetworkVnetSubnetUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CliFlag("--default-outbound")]
    public bool? DefaultOutbound { get; set; }

    [CliOption("--delegations")]
    public string? Delegations { get; set; }

    [CliFlag("--disable-private-endpoint-network-policies")]
    public bool? DisablePrivateEndpointNetworkPolicies { get; set; }

    [CliFlag("--disable-private-link-service-network-policies")]
    public bool? DisablePrivateLinkServiceNetworkPolicies { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--nat-gateway")]
    public string? NatGateway { get; set; }

    [CliOption("--network-security-group")]
    public string? NetworkSecurityGroup { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--route-table")]
    public string? RouteTable { get; set; }

    [CliOption("--service-endpoint-policy")]
    public string? ServiceEndpointPolicy { get; set; }

    [CliOption("--service-endpoints")]
    public string? ServiceEndpoints { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}