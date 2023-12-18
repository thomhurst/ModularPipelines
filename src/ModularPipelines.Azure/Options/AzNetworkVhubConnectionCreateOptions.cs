using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vhub", "connection", "create")]
public record AzNetworkVhubConnectionCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--remote-vnet")] string RemoteVnet,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vhub-name")] string VhubName
) : AzOptions
{
    [CommandSwitch("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CommandSwitch("--associated")]
    public string? Associated { get; set; }

    [CommandSwitch("--associated-inbound-routemap")]
    public string? AssociatedInboundRoutemap { get; set; }

    [CommandSwitch("--associated-outbound-routemap")]
    public string? AssociatedOutboundRoutemap { get; set; }

    [BooleanCommandSwitch("--internet-security")]
    public bool? InternetSecurity { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--next-hop")]
    public string? NextHop { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--propagated")]
    public string? Propagated { get; set; }

    [BooleanCommandSwitch("--remote-vnet-transit")]
    public bool? RemoteVnetTransit { get; set; }

    [CommandSwitch("--route-name")]
    public string? RouteName { get; set; }

    [BooleanCommandSwitch("--use-hub-vnet-gateways")]
    public bool? UseHubVnetGateways { get; set; }
}