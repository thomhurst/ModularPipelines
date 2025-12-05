using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("orbital", "available-ground-station", "show")]
public record AzOrbitalAvailableGroundStationShowOptions : AzOptions
{
    [CliOption("--ground-station-name")]
    public string? GroundStationName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}