using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "image", "list")]
public record AzSphereImageListOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;