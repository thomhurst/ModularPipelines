using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "traffic-manager", "endpoint", "show-geographic-hierarchy")]
public record AzNetworkTrafficManagerEndpointShowGeographicHierarchyOptions : AzOptions;