using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkfabric", "nni", "create")]
public record AzNetworkfabricNniCreateOptions(
[property: CliOption("--fabric")] string Fabric,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName,
[property: CliFlag("--use-option-b")] bool UseOptionB
) : AzOptions
{
    [CliOption("--egress-acl-id")]
    public string? EgressAclId { get; set; }

    [CliOption("--export-route-policy")]
    public string? ExportRoutePolicy { get; set; }

    [CliOption("--import-route-policy")]
    public string? ImportRoutePolicy { get; set; }

    [CliOption("--ingress-acl-id")]
    public string? IngressAclId { get; set; }

    [CliFlag("--is-management-type")]
    public bool? IsManagementType { get; set; }

    [CliOption("--layer2-configuration")]
    public string? Layer2Configuration { get; set; }

    [CliOption("--nni-type")]
    public string? NniType { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--npb-static-route-configuration")]
    public string? NpbStaticRouteConfiguration { get; set; }

    [CliOption("--option-b-layer3-configuration")]
    public string? OptionBLayer3Configuration { get; set; }
}