using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sig", "gallery-application", "version", "show")]
public record AzSigGalleryApplicationVersionShowOptions : AzOptions
{
    [CliOption("--application-name")]
    public string? ApplicationName { get; set; }

    [CliOption("--expand")]
    public string? Expand { get; set; }

    [CliOption("--gallery-application-version-name")]
    public string? GalleryApplicationVersionName { get; set; }

    [CliOption("--gallery-name")]
    public string? GalleryName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}