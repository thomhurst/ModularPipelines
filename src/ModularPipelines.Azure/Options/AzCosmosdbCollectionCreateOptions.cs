using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "collection", "create")]
public record AzCosmosdbCollectionCreateOptions(
[property: CommandSwitch("--collection-name")] string CollectionName,
[property: CommandSwitch("--db-name")] string DbName
) : AzOptions
{
    [CommandSwitch("--cep")]
    public string? Cep { get; set; }

    [CommandSwitch("--default-ttl")]
    public string? DefaultTtl { get; set; }

    [BooleanCommandSwitch("--indexing-policy")]
    public bool? IndexingPolicy { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--partition-key-path")]
    public string? PartitionKeyPath { get; set; }

    [CommandSwitch("--resource-group-name")]
    public string? ResourceGroupName { get; set; }

    [CommandSwitch("--throughput")]
    public string? Throughput { get; set; }

    [CommandSwitch("--url-connection")]
    public string? UrlConnection { get; set; }
}