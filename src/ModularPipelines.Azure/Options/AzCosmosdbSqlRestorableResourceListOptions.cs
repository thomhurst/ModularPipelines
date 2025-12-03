using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "sql", "restorable-resource", "list")]
public record AzCosmosdbSqlRestorableResourceListOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--location")] string Location,
[property: CliOption("--restore-location")] string RestoreLocation,
[property: CliOption("--restore-timestamp")] string RestoreTimestamp
) : AzOptions;