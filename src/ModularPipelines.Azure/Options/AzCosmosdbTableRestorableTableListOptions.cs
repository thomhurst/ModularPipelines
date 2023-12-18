using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "table", "restorable-table", "list")]
public record AzCosmosdbTableRestorableTableListOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}