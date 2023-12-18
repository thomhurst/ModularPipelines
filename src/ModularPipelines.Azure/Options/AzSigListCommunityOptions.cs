using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "list-community")]
public record AzSigListCommunityOptions : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--show-next-marker")]
    public string? ShowNextMarker { get; set; }
}