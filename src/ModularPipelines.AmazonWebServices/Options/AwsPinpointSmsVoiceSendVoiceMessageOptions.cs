using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice", "send-voice-message")]
public record AwsPinpointSmsVoiceSendVoiceMessageOptions : AwsOptions
{
    [CommandSwitch("--caller-id")]
    public string? CallerId { get; set; }

    [CommandSwitch("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CommandSwitch("--content")]
    public string? Content { get; set; }

    [CommandSwitch("--destination-phone-number")]
    public string? DestinationPhoneNumber { get; set; }

    [CommandSwitch("--origination-phone-number")]
    public string? OriginationPhoneNumber { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}