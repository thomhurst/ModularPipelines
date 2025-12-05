using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "create-fleet-locations")]
public record AwsGameliftCreateFleetLocationsOptions(
[property: CliOption("--fleet-id")] string FleetId,
[property: CliOption("--locations")] string[] Locations
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}