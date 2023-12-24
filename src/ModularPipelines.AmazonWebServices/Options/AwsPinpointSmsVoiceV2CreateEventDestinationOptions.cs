using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "create-event-destination")]
public record AwsPinpointSmsVoiceV2CreateEventDestinationOptions(
[property: CommandSwitch("--configuration-set-name")] string ConfigurationSetName,
[property: CommandSwitch("--event-destination-name")] string EventDestinationName,
[property: CommandSwitch("--matching-event-types")] string[] MatchingEventTypes
) : AwsOptions
{
    [CommandSwitch("--cloud-watch-logs-destination")]
    public string? CloudWatchLogsDestination { get; set; }

    [CommandSwitch("--kinesis-firehose-destination")]
    public string? KinesisFirehoseDestination { get; set; }

    [CommandSwitch("--sns-destination")]
    public string? SnsDestination { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}