using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sig", "image-definition", "list-community", "(image-gallery", "extension)")]
public record AzSigImageDefinitionListCommunityImageGalleryExtensionOptions : AzOptions
{
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