using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "gremlin", "graph", "list")]
public record AzCosmosdbGremlinGraphListOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--analytical-storage-ttl")]
    public string? AnalyticalStorageTtl { get; set; }

    [CommandSwitch("--idx")]
    public string? Idx { get; set; }

    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }
}