using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "gremlin", "restorable-database", "list", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbGremlinRestorableDatabaseListCosmosdbPreviewExtensionOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--location")] string Location
) : AzOptions;