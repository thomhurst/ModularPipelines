using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "table", "throughput", "update")]
public record AzCosmosdbTableThroughputUpdateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--max-throughput")]
    public string? MaxThroughput { get; set; }

    [CommandSwitch("--throughput")]
    public string? Throughput { get; set; }
}