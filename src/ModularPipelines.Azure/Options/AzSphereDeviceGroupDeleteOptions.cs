using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device-group", "delete")]
public record AzSphereDeviceGroupDeleteOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--device-group")] string DeviceGroup,
[property: CommandSwitch("--product")] string Product,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;