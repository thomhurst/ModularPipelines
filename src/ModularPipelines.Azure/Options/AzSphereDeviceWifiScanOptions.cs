using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "wifi", "scan")]
public record AzSphereDeviceWifiScanOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}