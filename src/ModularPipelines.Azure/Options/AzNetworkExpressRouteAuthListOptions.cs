using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "express-route", "auth", "list")]
public record AzNetworkExpressRouteAuthListOptions(
[property: CliOption("--circuit-name")] string CircuitName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;