using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("orbital", "available-ground-station", "list")]
public record AzOrbitalAvailableGroundStationListOptions : AzOptions
{
    [CommandSwitch("--capability")]
    public string? Capability { get; set; }
}