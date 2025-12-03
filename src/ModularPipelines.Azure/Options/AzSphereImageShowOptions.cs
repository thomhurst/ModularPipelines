using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "image", "show")]
public record AzSphereImageShowOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--image")] string Image,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;