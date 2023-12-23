using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "calculate-route")]
public record AwsLocationCalculateRouteOptions(
[property: CommandSwitch("--calculator-name")] string CalculatorName,
[property: CommandSwitch("--departure-position")] string[] DeparturePosition,
[property: CommandSwitch("--destination-position")] string[] DestinationPosition
) : AwsOptions
{
    [CommandSwitch("--arrival-time")]
    public long? ArrivalTime { get; set; }

    [CommandSwitch("--car-mode-options")]
    public string? CarModeOptions { get; set; }

    [CommandSwitch("--departure-time")]
    public long? DepartureTime { get; set; }

    [CommandSwitch("--distance-unit")]
    public string? DistanceUnit { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--optimize-for")]
    public string? OptimizeFor { get; set; }

    [CommandSwitch("--travel-mode")]
    public string? TravelMode { get; set; }

    [CommandSwitch("--truck-mode-options")]
    public string? TruckModeOptions { get; set; }

    [CommandSwitch("--waypoint-positions")]
    public string[]? WaypointPositions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}