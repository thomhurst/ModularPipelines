using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vnet", "subnet", "create")]
public record AzNetworkVnetSubnetCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vnet-name")] string VnetName
) : AzOptions
{
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

    [CliOption("--nat-gateway")]
    public string? NatGateway { get; set; }

    [CliOption("--network-security-group")]
    public string? NetworkSecurityGroup { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--route-table")]
    public string? RouteTable { get; set; }

    [CliOption("--service-endpoint-policy")]
    public string? ServiceEndpointPolicy { get; set; }

    [CliOption("--service-endpoints")]
    public string? ServiceEndpoints { get; set; }
}