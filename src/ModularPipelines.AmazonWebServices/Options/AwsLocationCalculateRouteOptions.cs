using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "calculate-route")]
public record AwsLocationCalculateRouteOptions(
[property: CliOption("--calculator-name")] string CalculatorName,
[property: CliOption("--departure-position")] string[] DeparturePosition,
[property: CliOption("--destination-position")] string[] DestinationPosition
) : AwsOptions
{
    [CliOption("--arrival-time")]
    public long? ArrivalTime { get; set; }

    [CliOption("--car-mode-options")]
    public string? CarModeOptions { get; set; }

    [CliOption("--departure-time")]
    public long? DepartureTime { get; set; }

    [CliOption("--distance-unit")]
    public string? DistanceUnit { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--optimize-for")]
    public string? OptimizeFor { get; set; }

    [CliOption("--travel-mode")]
    public string? TravelMode { get; set; }

    [CliOption("--truck-mode-options")]
    public string? TruckModeOptions { get; set; }

    [CliOption("--waypoint-positions")]
    public string[]? WaypointPositions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}