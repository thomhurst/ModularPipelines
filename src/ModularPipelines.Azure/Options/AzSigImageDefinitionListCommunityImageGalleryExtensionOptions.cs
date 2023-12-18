using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "image-definition", "list-community", "(image-gallery", "extension)")]
public record AzSigImageDefinitionListCommunityImageGalleryExtensionOptions : AzOptions
{
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
    public string? Subscription { get; set; }
}