using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "recover")]
public record AzSphereDeviceRecoverOptions : AzOptions
{
    [CommandSwitch("--capability")]
    public string? Capability { get; set; }

    [CommandSwitch("--device")]
    public string? Device { get; set; }

    [CommandSwitch("--images")]
    public string? Images { get; set; }
}