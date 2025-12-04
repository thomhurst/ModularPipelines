using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sig", "image-version", "show-community", "(image-gallery", "extension)")]
public record AzSigImageVersionShowCommunityImageGalleryExtensionOptions : AzOptions
{
    [CliOption("--gallery-image-definition")]
    public string? GalleryImageDefinition { get; set; }

    [CliOption("--gallery-image-version")]
    public string? GalleryImageVersion { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--public-gallery-name")]
    public string? PublicGalleryName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}