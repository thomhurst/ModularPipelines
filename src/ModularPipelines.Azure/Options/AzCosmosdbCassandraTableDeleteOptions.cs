using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "cassandra", "table", "delete")]
public record AzCosmosdbCassandraTableDeleteOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--keyspace-name")] string KeyspaceName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}