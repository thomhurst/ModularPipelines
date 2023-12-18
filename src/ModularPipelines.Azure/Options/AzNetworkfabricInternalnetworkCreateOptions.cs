using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "internalnetwork", "create")]
public record AzNetworkfabricInternalnetworkCreateOptions(
[property: CommandSwitch("--l3-isolation-domain-name")] string L3IsolationDomainName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--vlan-id")] string VlanId
) : AzOptions
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

    [CommandSwitch("--extension")]
    public string? Extension { get; set; }

    [CommandSwitch("--import-route-policy")]
    public string? ImportRoutePolicy { get; set; }

    [CommandSwitch("--import-route-policy-id")]
    public string? ImportRoutePolicyId { get; set; }

    [CommandSwitch("--ingress-acl-id")]
    public string? IngressAclId { get; set; }

    [BooleanCommandSwitch("--is-monitoring-enabled")]
    public bool? IsMonitoringEnabled { get; set; }

    [CommandSwitch("--mtu")]
    public string? Mtu { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--static-route-configuration")]
    public string? StaticRouteConfiguration { get; set; }
}