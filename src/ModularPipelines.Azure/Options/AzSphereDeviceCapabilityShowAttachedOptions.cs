using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "capability", "show-attached")]
public record AzSphereDeviceCapabilityShowAttachedOptions(
[property: CommandSwitch("--capability-file")] string CapabilityFile
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}