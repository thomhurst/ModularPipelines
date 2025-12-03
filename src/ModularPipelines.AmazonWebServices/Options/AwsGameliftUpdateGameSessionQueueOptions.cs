using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "update-game-session-queue")]
public record AwsGameliftUpdateGameSessionQueueOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--timeout-in-seconds")]
    public int? TimeoutInSeconds { get; set; }

    [CliOption("--player-latency-policies")]
    public string[]? PlayerLatencyPolicies { get; set; }

    [CliOption("--destinations")]
    public string[]? Destinations { get; set; }

    [CliOption("--filter-configuration")]
    public string? FilterConfiguration { get; set; }

    [CliOption("--priority-configuration")]
    public string? PriorityConfiguration { get; set; }

    [CliOption("--custom-event-data")]
    public string? CustomEventData { get; set; }

    [CliOption("--notification-target")]
    public string? NotificationTarget { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}