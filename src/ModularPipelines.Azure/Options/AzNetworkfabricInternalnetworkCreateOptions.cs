using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric", "internalnetwork", "create")]
public record AzNetworkfabricInternalnetworkCreateOptions(
[property: CliOption("--l3-isolation-domain-name")] string L3IsolationDomainName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--vlan-id")] string VlanId
) : AzOptions
{
    [CliOption("--annotation")]
    public string? Annotation { get; set; }

    [CliOption("--bgp-configuration")]
    public string? BgpConfiguration { get; set; }

    [CliOption("--connected-ipv4-subnets")]
    public string? ConnectedIpv4Subnets { get; set; }

    [CliOption("--connected-ipv6-subnets")]
    public string? ConnectedIpv6Subnets { get; set; }

    [CliOption("--egress-acl-id")]
    public string? EgressAclId { get; set; }

    [CliOption("--export-route-policy")]
    public string? ExportRoutePolicy { get; set; }

    [CliOption("--export-route-policy-id")]
    public string? ExportRoutePolicyId { get; set; }

    [CliOption("--extension")]
    public string? Extension { get; set; }

    [CliOption("--import-route-policy")]
    public string? ImportRoutePolicy { get; set; }

    [CliOption("--import-route-policy-id")]
    public string? ImportRoutePolicyId { get; set; }

    [CliOption("--ingress-acl-id")]
    public string? IngressAclId { get; set; }

    [CliFlag("--is-monitoring-enabled")]
    public bool? IsMonitoringEnabled { get; set; }

    [CliOption("--mtu")]
    public string? Mtu { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--static-route-configuration")]
    public string? StaticRouteConfiguration { get; set; }
}