using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb", "collection", "retrieve-partition-throughput")]
public record AzCosmosdbMongodbCollectionRetrievePartitionThroughputOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--all-partitions")]
    public bool? AllPartitions { get; set; }

    [CliOption("--physical-partition-ids")]
    public string? PhysicalPartitionIds { get; set; }
}