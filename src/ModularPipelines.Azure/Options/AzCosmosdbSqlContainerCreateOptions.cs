using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "container", "create")]
public record AzCosmosdbSqlContainerCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--partition-key-path")] string PartitionKeyPath,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--analytical-storage-ttl")]
    public string? AnalyticalStorageTtl { get; set; }

    [CommandSwitch("--cep")]
    public string? Cep { get; set; }

    [CommandSwitch("--conflict-resolution-policy")]
    public string? ConflictResolutionPolicy { get; set; }

    [BooleanCommandSwitch("--idx")]
    public bool? Idx { get; set; }

    [CommandSwitch("--max-throughput")]
    public string? MaxThroughput { get; set; }

    [CommandSwitch("--partition-key-version")]
    public string? PartitionKeyVersion { get; set; }

    [CommandSwitch("--throughput")]
    public string? Throughput { get; set; }

    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }

    [CommandSwitch("--unique-key-policy")]
    public string? UniqueKeyPolicy { get; set; }
}