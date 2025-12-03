using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "service-endpoint", "list")]
public record AzNetworkServiceEndpointListOptions(
[property: CliOption("--location")] string Location
) : AzOptions;