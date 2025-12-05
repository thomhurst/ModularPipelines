using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "cassandra", "table", "exists")]
public record AzCosmosdbCassandraTableExistsOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--keyspace-name")] string KeyspaceName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;