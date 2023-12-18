using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "image", "list-offers")]
public record AzVmImageListOffersOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--publisher")] string Publisher
) : AzOptions
{
    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }
}