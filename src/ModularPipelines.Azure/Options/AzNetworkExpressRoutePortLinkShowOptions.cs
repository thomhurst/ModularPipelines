using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "express-route", "port", "link", "show")]
public record AzNetworkExpressRoutePortLinkShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--port-name")] string PortName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;