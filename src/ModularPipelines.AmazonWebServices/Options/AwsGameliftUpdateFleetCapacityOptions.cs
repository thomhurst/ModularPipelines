using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "update-fleet-capacity")]
public record AwsGameliftUpdateFleetCapacityOptions(
[property: CommandSwitch("--fleet-id")] string FleetId
) : AwsOptions
{
    [CommandSwitch("--desired-instances")]
    public int? DesiredInstances { get; set; }

    [CommandSwitch("--min-size")]
    public int? MinSize { get; set; }

    [CommandSwitch("--max-size")]
    public int? MaxSize { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}