using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "update-game-server")]
public record AwsGameliftUpdateGameServerOptions(
[property: CommandSwitch("--game-server-group-name")] string GameServerGroupName,
[property: CommandSwitch("--game-server-id")] string GameServerId
) : AwsOptions
{
    [CommandSwitch("--game-server-data")]
    public string? GameServerData { get; set; }

    [CommandSwitch("--utilization-status")]
    public string? UtilizationStatus { get; set; }

    [CommandSwitch("--health-check")]
    public string? HealthCheck { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}