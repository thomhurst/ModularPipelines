using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "nni", "update")]
public record AzNetworkfabricNniUpdateOptions : AzOptions
{
    [CommandSwitch("--egress-acl-id")]
    public string? EgressAclId { get; set; }

    [CommandSwitch("--export-route-policy")]
    public string? ExportRoutePolicy { get; set; }

    [CommandSwitch("--fabric")]
    public string? Fabric { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--import-route-policy")]
    public string? ImportRoutePolicy { get; set; }

    [CommandSwitch("--ingress-acl-id")]
    public string? IngressAclId { get; set; }

    [CommandSwitch("--layer2-configuration")]
    public string? Layer2Configuration { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--npb-static-route-configuration")]
    public string? NpbStaticRouteConfiguration { get; set; }

    [CommandSwitch("--option-b-layer3-configuration")]
    public string? OptionBLayer3Configuration { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

