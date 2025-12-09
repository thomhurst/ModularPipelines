using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device-group", "create-defaults")]
public record AzSphereDeviceGroupCreateDefaultsOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--product")] string Product,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;