using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "image-version", "list-community", "(image-gallery", "extension)")]
public record AzSigImageVersionListCommunityImageGalleryExtensionOptions : AzOptions
{
    [CommandSwitch("--gallery-image-definition")]
    public string? GalleryImageDefinition { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--public-gallery-name")]
    public string? PublicGalleryName { get; set; }

    [CommandSwitch("--show-next-marker")]
    public string? ShowNextMarker { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}