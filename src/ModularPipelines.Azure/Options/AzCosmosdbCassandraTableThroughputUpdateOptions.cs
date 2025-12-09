using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "cassandra", "table", "throughput", "update")]
public record AzCosmosdbCassandraTableThroughputUpdateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--keyspace-name")] string KeyspaceName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--max-throughput")]
    public string? MaxThroughput { get; set; }

    [CliOption("--throughput")]
    public string? Throughput { get; set; }
}