using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "sql", "container", "redistribute-partition-throughput")]
public record AzCosmosdbSqlContainerRedistributePartitionThroughputOptions(
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