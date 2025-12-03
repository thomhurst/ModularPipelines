using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "sql", "container", "create")]
public record AzCosmosdbSqlContainerCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--name")] string Name,
[property: CliOption("--partition-key-path")] string PartitionKeyPath,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--analytical-storage-ttl")]
    public string? AnalyticalStorageTtl { get; set; }

    [CliOption("--cep")]
    public string? Cep { get; set; }

    [CliOption("--conflict-resolution-policy")]
    public string? ConflictResolutionPolicy { get; set; }

    [CliFlag("--idx")]
    public bool? Idx { get; set; }

    [CliOption("--max-throughput")]
    public string? MaxThroughput { get; set; }

    [CliOption("--partition-key-version")]
    public string? PartitionKeyVersion { get; set; }

    [CliOption("--throughput")]
    public string? Throughput { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }

    [CliOption("--unique-key-policy")]
    public string? UniqueKeyPolicy { get; set; }
}