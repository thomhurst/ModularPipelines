using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "cassandra", "table", "list")]
public record AzCosmosdbCassandraTableListOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--keyspace-name")] string KeyspaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;