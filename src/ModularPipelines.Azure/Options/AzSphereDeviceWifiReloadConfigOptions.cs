using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "wifi", "reload-config")]
public record AzSphereDeviceWifiReloadConfigOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}