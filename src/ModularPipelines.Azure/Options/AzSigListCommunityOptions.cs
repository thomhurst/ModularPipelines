using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sig", "list-community")]
public record AzSigListCommunityOptions : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--show-next-marker")]
    public string? ShowNextMarker { get; set; }
}