using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "update-fleet-capacity")]
public record AwsGameliftUpdateFleetCapacityOptions(
[property: CliOption("--fleet-id")] string FleetId
) : AwsOptions
{
    [CliOption("--desired-instances")]
    public int? DesiredInstances { get; set; }

    [CliOption("--min-size")]
    public int? MinSize { get; set; }

    [CliOption("--max-size")]
    public int? MaxSize { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}