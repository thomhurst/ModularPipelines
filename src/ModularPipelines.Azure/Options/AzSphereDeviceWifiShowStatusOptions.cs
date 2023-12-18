using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "wifi", "show-status")]
public record AzSphereDeviceWifiShowStatusOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}

