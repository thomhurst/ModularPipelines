using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "manufacturing-state", "show")]
public record AzSphereDeviceManufacturingStateShowOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}