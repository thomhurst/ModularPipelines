using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "externalnetwork", "create")]
public record AzNetworkfabricExternalnetworkCreateOptions(
[property: CommandSwitch("--l3-isolation-domain-name")] string L3IsolationDomainName,
[property: CommandSwitch("--peering-option")] string PeeringOption,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName
) : AzOptions
{
    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [CommandSwitch("--export-route-policy")]
    public string? ExportRoutePolicy { get; set; }

    [CommandSwitch("--export-route-policy-id")]
    public string? ExportRoutePolicyId { get; set; }

    [CommandSwitch("--import-route-policy")]
    public string? ImportRoutePolicy { get; set; }

    [CommandSwitch("--import-route-policy-id")]
    public string? ImportRoutePolicyId { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--option-a-properties")]
    public string? OptionAProperties { get; set; }

    [CommandSwitch("--option-b-properties")]
    public string? OptionBProperties { get; set; }
}