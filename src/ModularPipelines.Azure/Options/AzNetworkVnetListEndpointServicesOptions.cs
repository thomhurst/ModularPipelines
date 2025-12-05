using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vnet", "list-endpoint-services")]
public record AzNetworkVnetListEndpointServicesOptions(
[property: CliOption("--location")] string Location
) : AzOptions;