using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "start-game-session-placement")]
public record AwsGameliftStartGameSessionPlacementOptions(
[property: CliOption("--placement-id")] string PlacementId,
[property: CliOption("--game-session-queue-name")] string GameSessionQueueName,
[property: CliOption("--maximum-player-session-count")] int MaximumPlayerSessionCount
) : AwsOptions
{
    [CliOption("--game-properties")]
    public string[]? GameProperties { get; set; }

    [CliOption("--game-session-name")]
    public string? GameSessionName { get; set; }

    [CliOption("--player-latencies")]
    public string[]? PlayerLatencies { get; set; }

    [CliOption("--desired-player-sessions")]
    public string[]? DesiredPlayerSessions { get; set; }

    [CliOption("--game-session-data")]
    public string? GameSessionData { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}