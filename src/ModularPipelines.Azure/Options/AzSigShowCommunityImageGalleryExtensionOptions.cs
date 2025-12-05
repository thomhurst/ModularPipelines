using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sig", "show-community", "(image-gallery", "extension)")]
public record AzSigShowCommunityImageGalleryExtensionOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--public-gallery-name")]
    public string? PublicGalleryName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}