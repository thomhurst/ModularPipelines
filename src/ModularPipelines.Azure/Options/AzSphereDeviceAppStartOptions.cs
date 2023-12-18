using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

