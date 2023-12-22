using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "traffic-manager", "endpoint", "show-geographic-hierarchy")]
public record AzNetworkTrafficManagerEndpointShowGeographicHierarchyOptions : AzOptions;