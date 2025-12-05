using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "send-destination-number-verification-code")]
public record AwsPinpointSmsVoiceV2SendDestinationNumberVerificationCodeOptions(
[property: CliOption("--verified-destination-number-id")] string VerifiedDestinationNumberId,
[property: CliOption("--verification-channel")] string VerificationChannel
) : AwsOptions
{
    [CliOption("--language-code")]
    public string? LanguageCode { get; set; }

    [CliOption("--origination-identity")]
    public string? OriginationIdentity { get; set; }

    [CliOption("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CliOption("--context")]
    public IEnumerable<KeyValue>? Context { get; set; }

    [CliOption("--destination-country-parameters")]
    public IEnumerable<KeyValue>? DestinationCountryParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}