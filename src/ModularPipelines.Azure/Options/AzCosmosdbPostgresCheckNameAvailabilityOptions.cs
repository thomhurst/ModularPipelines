using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "postgres", "check-name-availability")]
public record AzCosmosdbPostgresCheckNameAvailabilityOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type
) : AzOptions;