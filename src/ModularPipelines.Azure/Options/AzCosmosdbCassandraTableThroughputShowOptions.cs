using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "cassandra", "table", "throughput", "show")]
public record AzCosmosdbCassandraTableThroughputShowOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--keyspace-name")] string KeyspaceName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;