using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "deployment", "list")]
public record AzSphereDeploymentListOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--device-group")] string DeviceGroup,
[property: CliOption("--product")] string Product,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;