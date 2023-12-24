using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "update-matchmaking-configuration")]
public record AwsGameliftUpdateMatchmakingConfigurationOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--game-session-queue-arns")]
    public string[]? GameSessionQueueArns { get; set; }

    [CommandSwitch("--request-timeout-seconds")]
    public int? RequestTimeoutSeconds { get; set; }

    [CommandSwitch("--acceptance-timeout-seconds")]
    public int? AcceptanceTimeoutSeconds { get; set; }

    [CommandSwitch("--rule-set-name")]
    public string? RuleSetName { get; set; }

    [CommandSwitch("--notification-target")]
    public string? NotificationTarget { get; set; }

    [CommandSwitch("--additional-player-count")]
    public int? AdditionalPlayerCount { get; set; }

    [CommandSwitch("--custom-event-data")]
    public string? CustomEventData { get; set; }

    [CommandSwitch("--game-properties")]
    public string[]? GameProperties { get; set; }

    [CommandSwitch("--game-session-data")]
    public string? GameSessionData { get; set; }

    [CommandSwitch("--backfill-mode")]
    public string? BackfillMode { get; set; }

    [CommandSwitch("--flex-match-mode")]
    public string? FlexMatchMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}