using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "deployment", "create")]
public record AzSphereDeploymentCreateOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--device-group")] string DeviceGroup,
[property: CommandSwitch("--images")] string Images,
[property: CommandSwitch("--product")] string Product,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;