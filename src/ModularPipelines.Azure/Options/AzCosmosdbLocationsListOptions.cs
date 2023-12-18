using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "locations", "list")]
public record AzCosmosdbLocationsListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}