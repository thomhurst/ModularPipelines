using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "product", "show")]
public record AzSphereProductShowOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--product")] string Product,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;