using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "app", "start")]
public record AzSphereDeviceAppStartOptions : AzOptions
{
    [CommandSwitch("--component-id")]
    public string? ComponentId { get; set; }

    [BooleanCommandSwitch("--debug-mode")]
    public bool? DebugMode { get; set; }

    [CommandSwitch("--device")]
    public string? Device { get; set; }
}