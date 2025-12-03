using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "express-route", "peering", "peer-connection", "list")]
public record AzNetworkExpressRoutePeeringPeerConnectionListOptions(
[property: CliOption("--circuit-name")] string CircuitName,
[property: CliOption("--peering-name")] string PeeringName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;