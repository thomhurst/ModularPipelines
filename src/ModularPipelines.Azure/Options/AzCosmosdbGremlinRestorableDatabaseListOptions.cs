using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "gremlin", "restorable-database", "list")]
public record AzCosmosdbGremlinRestorableDatabaseListOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--location")] string Location
) : AzOptions;