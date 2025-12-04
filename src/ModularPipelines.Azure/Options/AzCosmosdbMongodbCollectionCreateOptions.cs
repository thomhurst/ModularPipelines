using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "mongodb", "collection", "create")]
public record AzCosmosdbMongodbCollectionCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--analytical-storage-ttl")]
    public string? AnalyticalStorageTtl { get; set; }

    [CliOption("--idx")]
    public string? Idx { get; set; }

    [CliOption("--max-throughput")]
    public string? MaxThroughput { get; set; }

    [CliOption("--shard")]
    public string? Shard { get; set; }

    [CliOption("--throughput")]
    public string? Throughput { get; set; }
}