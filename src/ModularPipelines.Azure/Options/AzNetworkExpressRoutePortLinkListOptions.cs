using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "express-route", "port", "link", "list")]
public record AzNetworkExpressRoutePortLinkListOptions(
[property: CliOption("--port-name")] string PortName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;