using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "local-gateway", "create")]
public record AzNetworkLocalGatewayCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CliOption("--asn")]
    public string? Asn { get; set; }

    [CliOption("--bgp-peering-address")]
    public string? BgpPeeringAddress { get; set; }

    [CliOption("--gateway-ip-address")]
    public string? GatewayIpAddress { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--peer-weight")]
    public string? PeerWeight { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}