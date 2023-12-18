using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "restart")]
public record AzSphereDeviceRestartOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}