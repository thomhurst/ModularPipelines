using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "express-route", "peering", "connection", "create")]
public record AzNetworkExpressRoutePeeringConnectionCreateOptions(
[property: CliOption("--circuit-name")] string CircuitName,
[property: CliOption("--name")] string Name,
[property: CliOption("--peering-name")] string PeeringName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-prefix")]
    public string? AddressPrefix { get; set; }

    [CliOption("--authorization-key")]
    public string? AuthorizationKey { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--peer-circuit")]
    public string? PeerCircuit { get; set; }

    [CliOption("--source-circuit")]
    public string? SourceCircuit { get; set; }
}