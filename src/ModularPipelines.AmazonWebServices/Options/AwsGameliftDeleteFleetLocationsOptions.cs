using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "delete-fleet-locations")]
public record AwsGameliftDeleteFleetLocationsOptions(
[property: CommandSwitch("--fleet-id")] string FleetId,
[property: CommandSwitch("--locations")] string[] Locations
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}