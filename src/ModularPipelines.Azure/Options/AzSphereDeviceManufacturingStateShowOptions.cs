using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "manufacturing-state", "show")]
public record AzSphereDeviceManufacturingStateShowOptions(
[property: CommandSwitch("--state")] string State
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}