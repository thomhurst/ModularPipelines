using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric", "nni", "update")]
public record AzNetworkfabricNniUpdateOptions : AzOptions
{
    [CliOption("--egress-acl-id")]
    public string? EgressAclId { get; set; }

    [CliOption("--export-route-policy")]
    public string? ExportRoutePolicy { get; set; }

    [CliOption("--fabric")]
    public string? Fabric { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--import-route-policy")]
    public string? ImportRoutePolicy { get; set; }

    [CliOption("--ingress-acl-id")]
    public string? IngressAclId { get; set; }

    [CliOption("--layer2-configuration")]
    public string? Layer2Configuration { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--npb-static-route-configuration")]
    public string? NpbStaticRouteConfiguration { get; set; }

    [CliOption("--option-b-layer3-configuration")]
    public string? OptionBLayer3Configuration { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}