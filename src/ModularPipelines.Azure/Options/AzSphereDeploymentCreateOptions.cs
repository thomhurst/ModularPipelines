using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "deployment", "create")]
public record AzSphereDeploymentCreateOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--device-group")] string DeviceGroup,
[property: CliOption("--images")] string Images,
[property: CliOption("--product")] string Product,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;