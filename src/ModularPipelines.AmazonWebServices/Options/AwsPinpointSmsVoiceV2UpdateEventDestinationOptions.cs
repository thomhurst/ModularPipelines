using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "update-event-destination")]
public record AwsPinpointSmsVoiceV2UpdateEventDestinationOptions(
[property: CliOption("--configuration-set-name")] string ConfigurationSetName,
[property: CliOption("--event-destination-name")] string EventDestinationName
) : AwsOptions
{
    [CliOption("--matching-event-types")]
    public string[]? MatchingEventTypes { get; set; }

    [CliOption("--cloud-watch-logs-destination")]
    public string? CloudWatchLogsDestination { get; set; }

    [CliOption("--kinesis-firehose-destination")]
    public string? KinesisFirehoseDestination { get; set; }

    [CliOption("--sns-destination")]
    public string? SnsDestination { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}