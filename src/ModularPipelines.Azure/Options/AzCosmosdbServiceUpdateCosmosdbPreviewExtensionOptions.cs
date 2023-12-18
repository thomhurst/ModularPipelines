using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "service", "update", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbServiceUpdateCosmosdbPreviewExtensionOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--count")] int Count,
[property: CommandSwitch("--kind")] string Kind,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group-name")] string ResourceGroupName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }
}