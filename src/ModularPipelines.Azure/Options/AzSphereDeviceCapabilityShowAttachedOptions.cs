using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "capability", "show-attached")]
public record AzSphereDeviceCapabilityShowAttachedOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}