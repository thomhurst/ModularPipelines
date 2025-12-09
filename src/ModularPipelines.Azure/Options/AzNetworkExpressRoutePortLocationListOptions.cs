using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "express-route", "port", "location", "list")]
public record AzNetworkExpressRoutePortLocationListOptions : AzOptions;