using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "catalog", "delete")]
public record AzSphereCatalogDeleteOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;