using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "assign")]
public record AzSphereDeviceAssignOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--targeted-device-group")] string TargetedDeviceGroup
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }

    [CliOption("--device-group")]
    public string? DeviceGroup { get; set; }

    [CliOption("--product")]
    public string? Product { get; set; }
}