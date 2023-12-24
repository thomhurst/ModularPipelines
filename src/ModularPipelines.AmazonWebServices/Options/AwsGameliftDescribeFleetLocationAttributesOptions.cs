using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "describe-fleet-location-attributes")]
public record AwsGameliftDescribeFleetLocationAttributesOptions(
[property: CommandSwitch("--fleet-id")] string FleetId
) : AwsOptions
{
    [CommandSwitch("--locations")]
    public string[]? Locations { get; set; }

    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}