using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "wifi", "disable")]
public record AzSphereDeviceWifiDisableOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}