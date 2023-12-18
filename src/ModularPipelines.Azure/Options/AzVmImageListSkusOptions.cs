using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "image", "list-skus")]
public record AzVmImageListSkusOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--offer")] string Offer,
[property: CommandSwitch("--publisher")] string Publisher
) : AzOptions
{
    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }
}