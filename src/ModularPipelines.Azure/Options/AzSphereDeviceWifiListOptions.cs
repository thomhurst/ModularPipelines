using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "wifi", "list")]
public record AzSphereDeviceWifiListOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}