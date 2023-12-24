using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "start-game-session-placement")]
public record AwsGameliftStartGameSessionPlacementOptions(
[property: CommandSwitch("--placement-id")] string PlacementId,
[property: CommandSwitch("--game-session-queue-name")] string GameSessionQueueName,
[property: CommandSwitch("--maximum-player-session-count")] int MaximumPlayerSessionCount
) : AwsOptions
{
    [CommandSwitch("--game-properties")]
    public string[]? GameProperties { get; set; }

    [CommandSwitch("--game-session-name")]
    public string? GameSessionName { get; set; }

    [CommandSwitch("--player-latencies")]
    public string[]? PlayerLatencies { get; set; }

    [CommandSwitch("--desired-player-sessions")]
    public string[]? DesiredPlayerSessions { get; set; }

    [CommandSwitch("--game-session-data")]
    public string? GameSessionData { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}