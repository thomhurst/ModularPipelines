using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "wifi", "show-status")]
public record AzSphereDeviceWifiShowStatusOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}