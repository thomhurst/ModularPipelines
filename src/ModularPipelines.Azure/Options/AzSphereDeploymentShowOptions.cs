using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "deployment", "show")]
public record AzSphereDeploymentShowOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--deployment-id")] string DeploymentId,
[property: CliOption("--device-group")] string DeviceGroup,
[property: CliOption("--product")] string Product,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;