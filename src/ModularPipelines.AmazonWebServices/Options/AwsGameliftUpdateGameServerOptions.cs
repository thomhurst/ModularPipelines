using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "update-game-server")]
public record AwsGameliftUpdateGameServerOptions(
[property: CliOption("--game-server-group-name")] string GameServerGroupName,
[property: CliOption("--game-server-id")] string GameServerId
) : AwsOptions
{
    [CliOption("--game-server-data")]
    public string? GameServerData { get; set; }

    [CliOption("--utilization-status")]
    public string? UtilizationStatus { get; set; }

    [CliOption("--health-check")]
    public string? HealthCheck { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}