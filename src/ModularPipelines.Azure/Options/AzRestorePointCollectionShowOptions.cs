using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("restore-point", "collection", "show")]
public record AzRestorePointCollectionShowOptions(
[property: CommandSwitch("--collection-name")] string CollectionName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--restore-points")]
    public string? RestorePoints { get; set; }
}