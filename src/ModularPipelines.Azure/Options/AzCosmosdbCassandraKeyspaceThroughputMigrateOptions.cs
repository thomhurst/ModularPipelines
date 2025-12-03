using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "cassandra", "keyspace", "throughput", "migrate")]
public record AzCosmosdbCassandraKeyspaceThroughputMigrateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--throughput-type")] string ThroughputType
) : AzOptions;