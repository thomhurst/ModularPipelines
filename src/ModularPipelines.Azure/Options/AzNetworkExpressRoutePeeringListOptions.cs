using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "express-route", "peering", "list")]
public record AzNetworkExpressRoutePeeringListOptions(
[property: CliOption("--circuit-name")] string CircuitName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;