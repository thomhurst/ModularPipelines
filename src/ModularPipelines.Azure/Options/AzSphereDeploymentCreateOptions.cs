using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "deployment", "create")]
public record AzSphereDeploymentCreateOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--device-group")] string DeviceGroup,
[property: CommandSwitch("--images")] string Images,
[property: CommandSwitch("--product")] string Product,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}

