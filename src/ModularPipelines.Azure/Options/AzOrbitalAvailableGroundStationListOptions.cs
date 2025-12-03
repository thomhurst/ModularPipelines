using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("orbital", "available-ground-station", "list")]
public record AzOrbitalAvailableGroundStationListOptions : AzOptions
{
    [CliOption("--capability")]
    public string? Capability { get; set; }
}