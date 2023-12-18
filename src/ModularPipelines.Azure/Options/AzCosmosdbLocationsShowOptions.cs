using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "locations", "show")]
public record AzCosmosdbLocationsShowOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;