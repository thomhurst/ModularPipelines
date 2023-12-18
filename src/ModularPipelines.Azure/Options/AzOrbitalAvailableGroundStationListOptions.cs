using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("orbital", "available-ground-station", "list")]
public record AzOrbitalAvailableGroundStationListOptions : AzOptions
{
    [CommandSwitch("--capability")]
    public string? Capability { get; set; }
}