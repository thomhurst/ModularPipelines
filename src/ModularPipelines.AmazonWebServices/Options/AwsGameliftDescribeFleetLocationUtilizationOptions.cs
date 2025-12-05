using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "describe-fleet-location-utilization")]
public record AwsGameliftDescribeFleetLocationUtilizationOptions(
[property: CliOption("--fleet-id")] string FleetId,
[property: CliOption("--location")] string Location
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}