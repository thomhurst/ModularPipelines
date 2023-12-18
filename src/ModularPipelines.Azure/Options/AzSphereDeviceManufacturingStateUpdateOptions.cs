using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "manufacturing-state", "update")]
public record AzSphereDeviceManufacturingStateUpdateOptions(
[property: CommandSwitch("--state")] string State
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}