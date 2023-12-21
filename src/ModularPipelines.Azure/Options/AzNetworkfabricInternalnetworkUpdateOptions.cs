using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "internalnetwork", "update")]
public record AzNetworkfabricInternalnetworkUpdateOptions : AzOptions
{
    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [CommandSwitch("--bgp-configuration")]
    public string? BgpConfiguration { get; set; }

    [CommandSwitch("--connected-ipv4-subnets")]
    public string? ConnectedIpv4Subnets { get; set; }

    [CommandSwitch("--connected-ipv6-subnets")]
    public string? ConnectedIpv6Subnets { get; set; }

    [CommandSwitch("--egress-acl-id")]
    public string? EgressAclId { get; set; }

    [CommandSwitch("--export-route-policy")]
    public string? ExportRoutePolicy { get; set; }

    [CommandSwitch("--export-route-policy-id")]
    public string? ExportRoutePolicyId { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--import-route-policy")]
    public string? ImportRoutePolicy { get; set; }

    [CommandSwitch("--import-route-policy-id")]
    public string? ImportRoutePolicyId { get; set; }

    [CommandSwitch("--ingress-acl-id")]
    public string? IngressAclId { get; set; }

    [BooleanCommandSwitch("--is-monitoring-enabled")]
    public bool? IsMonitoringEnabled { get; set; }

    [CommandSwitch("--l3-isolation-domain-name")]
    public string? L3IsolationDomainName { get; set; }

    [CommandSwitch("--mtu")]
    public string? Mtu { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--static-route-configuration")]
    public string? StaticRouteConfiguration { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}