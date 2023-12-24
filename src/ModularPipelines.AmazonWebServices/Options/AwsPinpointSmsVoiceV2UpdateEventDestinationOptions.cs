using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "update-event-destination")]
public record AwsPinpointSmsVoiceV2UpdateEventDestinationOptions(
[property: CommandSwitch("--configuration-set-name")] string ConfigurationSetName,
[property: CommandSwitch("--event-destination-name")] string EventDestinationName
) : AwsOptions
{
    [CommandSwitch("--matching-event-types")]
    public string[]? MatchingEventTypes { get; set; }

    [CommandSwitch("--cloud-watch-logs-destination")]
    public string? CloudWatchLogsDestination { get; set; }

    [CommandSwitch("--kinesis-firehose-destination")]
    public string? KinesisFirehoseDestination { get; set; }

    [CommandSwitch("--sns-destination")]
    public string? SnsDestination { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}