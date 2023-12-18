using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "service", "create", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbServiceCreateCosmosdbPreviewExtensionOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--kind")] string Kind,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group-name")] string ResourceGroupName
) : AzOptions
{
    [CommandSwitch("--count")]
    public int? Count { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }
}

