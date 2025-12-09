using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "service-endpoint", "list")]
public record AzNetworkServiceEndpointListOptions(
[property: CliOption("--location")] string Location
) : AzOptions;