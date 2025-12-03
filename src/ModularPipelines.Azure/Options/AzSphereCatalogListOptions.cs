using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "catalog", "list")]
public record AzSphereCatalogListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;