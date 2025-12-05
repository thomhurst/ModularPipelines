using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "send-voice-message")]
public record AwsPinpointSmsVoiceV2SendVoiceMessageOptions(
[property: CliOption("--destination-phone-number")] string DestinationPhoneNumber,
[property: CliOption("--origination-identity")] string OriginationIdentity
) : AwsOptions
{
    [CliOption("--message-body")]
    public string? MessageBody { get; set; }

    [CliOption("--message-body-text-type")]
    public string? MessageBodyTextType { get; set; }

    [CliOption("--voice-id")]
    public string? VoiceId { get; set; }

    [CliOption("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CliOption("--max-price-per-minute")]
    public string? MaxPricePerMinute { get; set; }

    [CliOption("--time-to-live")]
    public int? TimeToLive { get; set; }

    [CliOption("--context")]
    public IEnumerable<KeyValue>? Context { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}