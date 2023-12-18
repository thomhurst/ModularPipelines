using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "service", "update")]
public record AzCosmosdbServiceUpdateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--count")] int Count,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group-name")] string ResourceGroupName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }
}