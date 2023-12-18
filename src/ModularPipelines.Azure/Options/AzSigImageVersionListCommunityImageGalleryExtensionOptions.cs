using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "image-version", "list-community", "(image-gallery", "extension)")]
public record AzSigImageVersionListCommunityImageGalleryExtensionOptions(
[property: CommandSwitch("--gallery-image-definition")] string GalleryImageDefinition,
[property: CommandSwitch("--gallery-image-version")] string GalleryImageVersion,
[property: CommandSwitch("--gallery-name")] string GalleryName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
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

