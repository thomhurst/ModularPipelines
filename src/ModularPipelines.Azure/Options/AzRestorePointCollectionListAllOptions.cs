using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("restore-point", "collection", "list-all")]
public record AzRestorePointCollectionListAllOptions(
[property: CommandSwitch("--collection-name")] string CollectionName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--restore-points")]
    public string? RestorePoints { get; set; }
}

