using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice", "send-voice-message")]
public record AwsPinpointSmsVoiceSendVoiceMessageOptions : AwsOptions
{
    [CliOption("--caller-id")]
    public string? CallerId { get; set; }

    [CliOption("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CliOption("--content")]
    public string? Content { get; set; }

    [CliOption("--destination-phone-number")]
    public string? DestinationPhoneNumber { get; set; }

    [CliOption("--origination-phone-number")]
    public string? OriginationPhoneNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}