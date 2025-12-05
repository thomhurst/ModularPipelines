using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "create-event-destination")]
public record AwsPinpointSmsVoiceV2CreateEventDestinationOptions(
[property: CliOption("--configuration-set-name")] string ConfigurationSetName,
[property: CliOption("--event-destination-name")] string EventDestinationName,
[property: CliOption("--matching-event-types")] string[] MatchingEventTypes
) : AwsOptions
{
    [CliOption("--cloud-watch-logs-destination")]
    public string? CloudWatchLogsDestination { get; set; }

    [CliOption("--kinesis-firehose-destination")]
    public string? KinesisFirehoseDestination { get; set; }

    [CliOption("--sns-destination")]
    public string? SnsDestination { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}