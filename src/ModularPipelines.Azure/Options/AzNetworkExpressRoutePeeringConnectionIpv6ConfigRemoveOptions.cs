using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "express-route", "peering", "connection", "ipv6-config", "remove")]
public record AzNetworkExpressRoutePeeringConnectionIpv6ConfigRemoveOptions(
[property: CliOption("--circuit-name")] string CircuitName,
[property: CliOption("--name")] string Name,
[property: CliOption("--peering-name")] string PeeringName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}