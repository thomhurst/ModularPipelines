using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sig", "show")]
public record AzSigShowOptions : AzOptions
{
    [CliOption("--gallery-name")]
    public string? GalleryName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--select")]
    public string? Select { get; set; }

    [CliOption("--sharing-groups")]
    public string? SharingGroups { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}