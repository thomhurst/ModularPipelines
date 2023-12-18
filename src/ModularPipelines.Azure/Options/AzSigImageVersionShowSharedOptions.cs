using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "image-version", "show-shared")]
public record AzSigImageVersionShowSharedOptions : AzOptions
{
    [CommandSwitch("--gallery-image-definition")]
    public string? GalleryImageDefinition { get; set; }

    [CommandSwitch("--gallery-image-version")]
    public string? GalleryImageVersion { get; set; }

    [CommandSwitch("--gallery-unique-name")]
    public string? GalleryUniqueName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}