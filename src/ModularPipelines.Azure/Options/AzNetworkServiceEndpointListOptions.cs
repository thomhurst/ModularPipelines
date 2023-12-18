using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "service-endpoint", "list")]
public record AzNetworkServiceEndpointListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}