using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "express-route", "peering", "connection", "ipv6-config", "set")]
public record AzNetworkExpressRoutePeeringConnectionIpv6ConfigSetOptions(
[property: CliOption("--circuit-name")] string CircuitName,
[property: CliOption("--name")] string Name,
[property: CliOption("--peering-name")] string PeeringName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-prefix")]
    public string? AddressPrefix { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}