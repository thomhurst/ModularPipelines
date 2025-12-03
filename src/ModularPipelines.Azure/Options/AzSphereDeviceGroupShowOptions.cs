using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device-group", "show")]
public record AzSphereDeviceGroupShowOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--device-group")] string DeviceGroup,
[property: CliOption("--product")] string Product,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;