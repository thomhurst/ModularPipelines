using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "send-destination-number-verification-code")]
public record AwsPinpointSmsVoiceV2SendDestinationNumberVerificationCodeOptions(
[property: CommandSwitch("--verified-destination-number-id")] string VerifiedDestinationNumberId,
[property: CommandSwitch("--verification-channel")] string VerificationChannel
) : AwsOptions
{
    [CommandSwitch("--language-code")]
    public string? LanguageCode { get; set; }

    [CommandSwitch("--origination-identity")]
    public string? OriginationIdentity { get; set; }

    [CommandSwitch("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CommandSwitch("--context")]
    public IEnumerable<KeyValue>? Context { get; set; }

    [CommandSwitch("--destination-country-parameters")]
    public IEnumerable<KeyValue>? DestinationCountryParameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}