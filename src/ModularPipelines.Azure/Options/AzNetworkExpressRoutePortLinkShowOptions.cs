using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "port", "link", "show")]
public record AzNetworkExpressRoutePortLinkShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--port-name")] string PortName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;