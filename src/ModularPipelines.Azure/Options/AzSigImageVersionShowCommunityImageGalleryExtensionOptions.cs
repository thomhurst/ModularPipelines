using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "image-version", "show-community", "(image-gallery", "extension)")]
public record AzSigImageVersionShowCommunityImageGalleryExtensionOptions : AzOptions
{
    [CommandSwitch("--gallery-image-definition")]
    public string? GalleryImageDefinition { get; set; }

    [CommandSwitch("--gallery-image-version")]
    public string? GalleryImageVersion { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--public-gallery-name")]
    public string? PublicGalleryName { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}