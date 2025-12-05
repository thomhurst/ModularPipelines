using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "image", "add")]
public record AzSphereImageAddOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--image-path")] string ImagePath,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--regional-data-boundary")]
    public string? RegionalDataBoundary { get; set; }
}