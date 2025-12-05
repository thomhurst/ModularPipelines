using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "locations", "show")]
public record AzCosmosdbLocationsShowOptions(
[property: CliOption("--location")] string Location
) : AzOptions;