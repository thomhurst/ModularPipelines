using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "table", "restorable-table", "list", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbTableRestorableTableListCosmosdbPreviewExtensionOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}

