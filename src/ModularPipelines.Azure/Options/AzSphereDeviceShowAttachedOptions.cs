using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "show-attached")]
public record AzSphereDeviceShowAttachedOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}