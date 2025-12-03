using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkfabric", "internalnetwork", "update")]
public record AzNetworkfabricInternalnetworkUpdateOptions : AzOptions
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

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--import-route-policy")]
    public string? ImportRoutePolicy { get; set; }

    [CliOption("--import-route-policy-id")]
    public string? ImportRoutePolicyId { get; set; }

    [CliOption("--ingress-acl-id")]
    public string? IngressAclId { get; set; }

    [CliFlag("--is-monitoring-enabled")]
    public bool? IsMonitoringEnabled { get; set; }

    [CliOption("--l3-isolation-domain-name")]
    public string? L3IsolationDomainName { get; set; }

    [CliOption("--mtu")]
    public string? Mtu { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--static-route-configuration")]
    public string? StaticRouteConfiguration { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}