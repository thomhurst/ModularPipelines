using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "adaptive-application-controls", "show")]
public record AzSecurityAdaptiveApplicationControlsShowOptions(
[property: CliOption("--group-name")] string GroupName
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}