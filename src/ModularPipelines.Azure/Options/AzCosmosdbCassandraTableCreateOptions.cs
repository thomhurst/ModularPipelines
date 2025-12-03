using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "cassandra", "table", "create")]
public record AzCosmosdbCassandraTableCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--keyspace-name")] string KeyspaceName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--schema")] string Schema
) : AzOptions
{
    [CliOption("--analytical-storage-ttl")]
    public string? AnalyticalStorageTtl { get; set; }

    [CliOption("--max-throughput")]
    public string? MaxThroughput { get; set; }

    [CliOption("--throughput")]
    public string? Throughput { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }
}