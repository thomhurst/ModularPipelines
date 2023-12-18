using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "capability", "update")]
public record AzSphereDeviceCapabilityUpdateOptions(
[property: CommandSwitch("--capability-file")] string CapabilityFile
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}