using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sig", "image-version", "list-shared")]
public record AzSigImageVersionListSharedOptions : AzOptions
{
    [CliOption("--gallery-image-definition")]
    public string? GalleryImageDefinition { get; set; }

    [CliOption("--gallery-unique-name")]
    public string? GalleryUniqueName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--shared-to")]
    public string? SharedTo { get; set; }

    [CliOption("--show-next-marker")]
    public string? ShowNextMarker { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}