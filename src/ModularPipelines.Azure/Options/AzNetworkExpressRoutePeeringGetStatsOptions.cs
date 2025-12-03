using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "express-route", "peering", "get-stats")]
public record AzNetworkExpressRoutePeeringGetStatsOptions(
[property: CliOption("--circuit-name")] string CircuitName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;