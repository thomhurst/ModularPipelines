using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "update-matchmaking-configuration")]
public record AwsGameliftUpdateMatchmakingConfigurationOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--game-session-queue-arns")]
    public string[]? GameSessionQueueArns { get; set; }

    [CliOption("--request-timeout-seconds")]
    public int? RequestTimeoutSeconds { get; set; }

    [CliOption("--acceptance-timeout-seconds")]
    public int? AcceptanceTimeoutSeconds { get; set; }

    [CliOption("--rule-set-name")]
    public string? RuleSetName { get; set; }

    [CliOption("--notification-target")]
    public string? NotificationTarget { get; set; }

    [CliOption("--additional-player-count")]
    public int? AdditionalPlayerCount { get; set; }

    [CliOption("--custom-event-data")]
    public string? CustomEventData { get; set; }

    [CliOption("--game-properties")]
    public string[]? GameProperties { get; set; }

    [CliOption("--game-session-data")]
    public string? GameSessionData { get; set; }

    [CliOption("--backfill-mode")]
    public string? BackfillMode { get; set; }

    [CliOption("--flex-match-mode")]
    public string? FlexMatchMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}