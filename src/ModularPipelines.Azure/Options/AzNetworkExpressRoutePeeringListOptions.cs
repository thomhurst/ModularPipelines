using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "peering", "list")]
public record AzNetworkExpressRoutePeeringListOptions(
[property: CommandSwitch("--circuit-name")] string CircuitName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;