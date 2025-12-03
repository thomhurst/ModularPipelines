using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sig", "image-version", "show")]
public record AzSigImageVersionShowOptions(
[property: CliOption("--gallery-image-definition")] string GalleryImageDefinition,
[property: CliOption("--gallery-image-version")] string GalleryImageVersion,
[property: CliOption("--gallery-name")] string GalleryName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }
}