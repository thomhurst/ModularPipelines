using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("orbital", "available-ground-station", "show")]
public record AzOrbitalAvailableGroundStationShowOptions : AzOptions
{
    [CommandSwitch("--ground-station-name")]
    public string? GroundStationName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}