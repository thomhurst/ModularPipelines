using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "deployment", "show")]
public record AzSphereDeploymentShowOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--deployment-id")] string DeploymentId,
[property: CommandSwitch("--device-group")] string DeviceGroup,
[property: CommandSwitch("--product")] string Product,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;