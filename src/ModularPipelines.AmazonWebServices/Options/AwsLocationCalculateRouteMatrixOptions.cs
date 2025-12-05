using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "calculate-route-matrix")]
public record AwsLocationCalculateRouteMatrixOptions(
[property: CliOption("--calculator-name")] string CalculatorName,
[property: CliOption("--departure-positions")] string[] DeparturePositions,
[property: CliOption("--destination-positions")] string[] DestinationPositions
) : AwsOptions
{
    [CliOption("--car-mode-options")]
    public string? CarModeOptions { get; set; }

    [CliOption("--departure-time")]
    public long? DepartureTime { get; set; }

    [CliOption("--distance-unit")]
    public string? DistanceUnit { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--travel-mode")]
    public string? TravelMode { get; set; }

    [CliOption("--truck-mode-options")]
    public string? TruckModeOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}