using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "update-game-session-queue")]
public record AwsGameliftUpdateGameSessionQueueOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--timeout-in-seconds")]
    public int? TimeoutInSeconds { get; set; }

    [CommandSwitch("--player-latency-policies")]
    public string[]? PlayerLatencyPolicies { get; set; }

    [CommandSwitch("--destinations")]
    public string[]? Destinations { get; set; }

    [CommandSwitch("--filter-configuration")]
    public string? FilterConfiguration { get; set; }

    [CommandSwitch("--priority-configuration")]
    public string? PriorityConfiguration { get; set; }

    [CommandSwitch("--custom-event-data")]
    public string? CustomEventData { get; set; }

    [CommandSwitch("--notification-target")]
    public string? NotificationTarget { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}