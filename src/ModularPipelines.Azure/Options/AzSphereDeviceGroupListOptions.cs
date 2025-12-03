using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device-group", "list")]
public record AzSphereDeviceGroupListOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--device-group")]
    public string? DeviceGroup { get; set; }

    [CliOption("--product")]
    public string? Product { get; set; }
}