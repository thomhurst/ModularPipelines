using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet", "list-endpoint-services")]
public record AzNetworkVnetListEndpointServicesOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;