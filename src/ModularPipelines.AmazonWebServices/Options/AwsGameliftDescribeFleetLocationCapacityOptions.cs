using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "describe-fleet-location-capacity")]
public record AwsGameliftDescribeFleetLocationCapacityOptions(
[property: CommandSwitch("--fleet-id")] string FleetId,
[property: CommandSwitch("--location")] string Location
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}