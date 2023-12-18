using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "collection", "create")]
public record AzCosmosdbMongodbCollectionCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--analytical-storage-ttl")]
    public string? AnalyticalStorageTtl { get; set; }

    [CommandSwitch("--idx")]
    public string? Idx { get; set; }

    [CommandSwitch("--max-throughput")]
    public string? MaxThroughput { get; set; }

    [CommandSwitch("--shard")]
    public string? Shard { get; set; }

    [CommandSwitch("--throughput")]
    public string? Throughput { get; set; }
}