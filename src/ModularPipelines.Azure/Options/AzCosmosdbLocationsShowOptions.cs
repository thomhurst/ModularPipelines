using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "locations", "show")]
public record AzCosmosdbLocationsShowOptions(
[property: CliOption("--location")] string Location
) : AzOptions;