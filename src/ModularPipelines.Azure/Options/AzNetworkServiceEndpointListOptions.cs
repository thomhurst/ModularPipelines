using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "service-endpoint", "list")]
public record AzNetworkServiceEndpointListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;