using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "product", "update")]
public record AzSphereProductUpdateOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--description")] string Description,
[property: CliOption("--product")] string Product,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;