using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "calculate-route-matrix")]
public record AwsLocationCalculateRouteMatrixOptions(
[property: CommandSwitch("--calculator-name")] string CalculatorName,
[property: CommandSwitch("--departure-positions")] string[] DeparturePositions,
[property: CommandSwitch("--destination-positions")] string[] DestinationPositions
) : AwsOptions
{
    [CommandSwitch("--car-mode-options")]
    public string? CarModeOptions { get; set; }

    [CommandSwitch("--departure-time")]
    public long? DepartureTime { get; set; }

    [CommandSwitch("--distance-unit")]
    public string? DistanceUnit { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--travel-mode")]
    public string? TravelMode { get; set; }

    [CommandSwitch("--truck-mode-options")]
    public string? TruckModeOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}