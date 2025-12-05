using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "mongodb", "restorable-resource", "list")]
public record AzCosmosdbMongodbRestorableResourceListOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--location")] string Location,
[property: CliOption("--restore-location")] string RestoreLocation,
[property: CliOption("--restore-timestamp")] string RestoreTimestamp
) : AzOptions;