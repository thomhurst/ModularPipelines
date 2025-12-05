using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "describe-fleet-port-settings")]
public record AwsGameliftDescribeFleetPortSettingsOptions(
[property: CliOption("--fleet-id")] string FleetId
) : AwsOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}