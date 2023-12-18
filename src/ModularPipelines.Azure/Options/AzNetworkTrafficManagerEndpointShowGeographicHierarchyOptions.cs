using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "traffic-manager", "endpoint", "show-geographic-hierarchy")]
public record AzNetworkTrafficManagerEndpointShowGeographicHierarchyOptions : AzOptions
{
}