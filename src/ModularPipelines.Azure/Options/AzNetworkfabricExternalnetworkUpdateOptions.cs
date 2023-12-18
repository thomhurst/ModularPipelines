using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "externalnetwork", "update")]
public record AzNetworkfabricExternalnetworkUpdateOptions : AzOptions
{
    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

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

    [CommandSwitch("--l3-isolation-domain-name")]
    public string? L3IsolationDomainName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--option-a-properties")]
    public string? OptionAProperties { get; set; }

    [CommandSwitch("--option-b-properties")]
    public string? OptionBProperties { get; set; }

    [CommandSwitch("--peering-option")]
    public string? PeeringOption { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}