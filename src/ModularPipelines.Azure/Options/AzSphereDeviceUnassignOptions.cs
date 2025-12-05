using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "unassign")]
public record AzSphereDeviceUnassignOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--device-group")] string DeviceGroup,
[property: CliOption("--product")] string Product,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}