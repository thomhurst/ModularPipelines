using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "wifi", "scan")]
public record AzSphereDeviceWifiScanOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}