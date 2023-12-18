using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "nni", "create")]
public record AzNetworkfabricNniCreateOptions(
[property: CommandSwitch("--fabric")] string Fabric,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: BooleanCommandSwitch("--use-option-b")] bool UseOptionB
) : AzOptions
{
    [CommandSwitch("--egress-acl-id")]
    public string? EgressAclId { get; set; }

    [CommandSwitch("--export-route-policy")]
    public string? ExportRoutePolicy { get; set; }

    [CommandSwitch("--import-route-policy")]
    public string? ImportRoutePolicy { get; set; }

    [CommandSwitch("--ingress-acl-id")]
    public string? IngressAclId { get; set; }

    [BooleanCommandSwitch("--is-management-type")]
    public bool? IsManagementType { get; set; }

    [CommandSwitch("--layer2-configuration")]
    public string? Layer2Configuration { get; set; }

    [CommandSwitch("--nni-type")]
    public string? NniType { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--npb-static-route-configuration")]
    public string? NpbStaticRouteConfiguration { get; set; }

    [CommandSwitch("--option-b-layer3-configuration")]
    public string? OptionBLayer3Configuration { get; set; }
}