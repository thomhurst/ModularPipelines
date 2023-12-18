using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "wifi", "reload-config")]
public record AzSphereDeviceWifiReloadConfigOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}