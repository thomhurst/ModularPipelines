using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "cassandra", "table", "list")]
public record AzCosmosdbCassandraTableListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--keyspace-name")] string KeyspaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;