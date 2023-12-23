using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "send-text-message")]
public record AwsPinpointSmsVoiceV2SendTextMessageOptions(
[property: CommandSwitch("--destination-phone-number")] string DestinationPhoneNumber
) : AwsOptions
{
    [CommandSwitch("--origination-identity")]
    public string? OriginationIdentity { get; set; }

    [CommandSwitch("--message-body")]
    public string? MessageBody { get; set; }

    [CommandSwitch("--message-type")]
    public string? MessageType { get; set; }

    [CommandSwitch("--keyword")]
    public string? Keyword { get; set; }

    [CommandSwitch("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CommandSwitch("--max-price")]
    public string? MaxPrice { get; set; }

    [CommandSwitch("--time-to-live")]
    public int? TimeToLive { get; set; }

    [CommandSwitch("--context")]
    public IEnumerable<KeyValue>? Context { get; set; }

    [CommandSwitch("--destination-country-parameters")]
    public IEnumerable<KeyValue>? DestinationCountryParameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}