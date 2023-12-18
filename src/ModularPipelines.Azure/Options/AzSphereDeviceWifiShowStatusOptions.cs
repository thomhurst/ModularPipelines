using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "wifi", "show-status")]
public record AzSphereDeviceWifiShowStatusOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}