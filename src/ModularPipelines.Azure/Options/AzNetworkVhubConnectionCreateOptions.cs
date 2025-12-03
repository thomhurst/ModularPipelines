using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vhub", "connection", "create")]
public record AzNetworkVhubConnectionCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--remote-vnet")] string RemoteVnet,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vhub-name")] string VhubName
) : AzOptions
{
    [CliOption("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CliOption("--associated")]
    public string? Associated { get; set; }

    [CliOption("--associated-inbound-routemap")]
    public string? AssociatedInboundRoutemap { get; set; }

    [CliOption("--associated-outbound-routemap")]
    public string? AssociatedOutboundRoutemap { get; set; }

    [CliFlag("--internet-security")]
    public bool? InternetSecurity { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--next-hop")]
    public string? NextHop { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--propagated")]
    public string? Propagated { get; set; }

    [CliFlag("--remote-vnet-transit")]
    public bool? RemoteVnetTransit { get; set; }

    [CliOption("--route-name")]
    public string? RouteName { get; set; }

    [CliFlag("--use-hub-vnet-gateways")]
    public bool? UseHubVnetGateways { get; set; }
}