using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "collection", "exists")]
public record AzCosmosdbMongodbCollectionExistsOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--evenly-distribute")]
    public bool? EvenlyDistribute { get; set; }

    [CommandSwitch("--source-partition-info")]
    public string? SourcePartitionInfo { get; set; }

    [CommandSwitch("--target-partition-info")]
    public string? TargetPartitionInfo { get; set; }
}