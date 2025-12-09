using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "product", "create")]
public record AzSphereProductCreateOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--description")] string Description,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;