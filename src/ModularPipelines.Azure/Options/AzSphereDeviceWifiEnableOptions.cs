using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "wifi", "enable")]
public record AzSphereDeviceWifiEnableOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}