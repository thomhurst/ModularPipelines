using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb", "collection", "redistribute-partition-throughput")]
public record AzCosmosdbMongodbCollectionRedistributePartitionThroughputOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--evenly-distribute")]
    public bool? EvenlyDistribute { get; set; }

    [CliOption("--source-partition-info")]
    public string? SourcePartitionInfo { get; set; }

    [CliOption("--target-partition-info")]
    public string? TargetPartitionInfo { get; set; }
}