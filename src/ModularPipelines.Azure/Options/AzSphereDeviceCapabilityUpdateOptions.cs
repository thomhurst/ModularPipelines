using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "capability", "update")]
public record AzSphereDeviceCapabilityUpdateOptions(
[property: CommandSwitch("--capability-file")] string CapabilityFile
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}