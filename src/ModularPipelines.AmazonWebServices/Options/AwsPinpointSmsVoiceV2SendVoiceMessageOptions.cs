using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "send-voice-message")]
public record AwsPinpointSmsVoiceV2SendVoiceMessageOptions(
[property: CommandSwitch("--destination-phone-number")] string DestinationPhoneNumber,
[property: CommandSwitch("--origination-identity")] string OriginationIdentity
) : AwsOptions
{
    [CommandSwitch("--message-body")]
    public string? MessageBody { get; set; }

    [CommandSwitch("--message-body-text-type")]
    public string? MessageBodyTextType { get; set; }

    [CommandSwitch("--voice-id")]
    public string? VoiceId { get; set; }

    [CommandSwitch("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CommandSwitch("--max-price-per-minute")]
    public string? MaxPricePerMinute { get; set; }

    [CommandSwitch("--time-to-live")]
    public int? TimeToLive { get; set; }

    [CommandSwitch("--context")]
    public IEnumerable<KeyValue>? Context { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}