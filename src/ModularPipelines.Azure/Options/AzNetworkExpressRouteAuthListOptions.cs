using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "auth", "list")]
public record AzNetworkExpressRouteAuthListOptions(
[property: CommandSwitch("--circuit-name")] string CircuitName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;