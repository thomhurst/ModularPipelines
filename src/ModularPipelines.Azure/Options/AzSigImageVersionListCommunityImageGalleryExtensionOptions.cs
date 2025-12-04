using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sig", "image-version", "list-community", "(image-gallery", "extension)")]
public record AzSigImageVersionListCommunityImageGalleryExtensionOptions : AzOptions
{
    [CliOption("--gallery-image-definition")]
    public string? GalleryImageDefinition { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--public-gallery-name")]
    public string? PublicGalleryName { get; set; }

    [CliOption("--show-next-marker")]
    public string? ShowNextMarker { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}