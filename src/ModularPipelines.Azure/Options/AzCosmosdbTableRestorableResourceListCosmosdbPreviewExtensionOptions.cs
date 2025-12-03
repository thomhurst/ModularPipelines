using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "table", "restorable-resource", "list", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbTableRestorableResourceListCosmosdbPreviewExtensionOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--location")] string Location,
[property: CliOption("--restore-location")] string RestoreLocation,
[property: CliOption("--restore-timestamp")] string RestoreTimestamp
) : AzOptions;