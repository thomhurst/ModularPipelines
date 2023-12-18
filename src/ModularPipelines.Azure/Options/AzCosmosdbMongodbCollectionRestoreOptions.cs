using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "collection", "restore")]
public record AzCosmosdbMongodbCollectionRestoreOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--restore-timestamp")] string RestoreTimestamp
) : AzOptions
{
    [BooleanCommandSwitch("--all-partitions")]
    public bool? AllPartitions { get; set; }

    [CommandSwitch("--physical-partition-ids")]
    public string? PhysicalPartitionIds { get; set; }
}