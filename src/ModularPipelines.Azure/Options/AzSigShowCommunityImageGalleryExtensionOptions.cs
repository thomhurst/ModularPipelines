using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "show-community", "(image-gallery", "extension)")]
public record AzSigShowCommunityImageGalleryExtensionOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--public-gallery-name")]
    public string? PublicGalleryName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}