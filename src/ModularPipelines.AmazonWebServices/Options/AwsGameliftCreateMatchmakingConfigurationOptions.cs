using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "create-matchmaking-configuration")]
public record AwsGameliftCreateMatchmakingConfigurationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--request-timeout-seconds")] int RequestTimeoutSeconds,
[property: CommandSwitch("--rule-set-name")] string RuleSetName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--game-session-queue-arns")]
    public string[]? GameSessionQueueArns { get; set; }

    [CommandSwitch("--acceptance-timeout-seconds")]
    public int? AcceptanceTimeoutSeconds { get; set; }

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

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}