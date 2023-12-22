using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "list-service-providers")]
public record AzNetworkExpressRouteListServiceProvidersOptions : AzOptions;