using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "image", "list-publishers")]
public record AzVmImageListPublishersOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }
}