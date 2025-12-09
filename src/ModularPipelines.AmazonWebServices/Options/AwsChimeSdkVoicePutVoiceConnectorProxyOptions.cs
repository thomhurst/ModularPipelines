using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "put-voice-connector-proxy")]
public record AwsChimeSdkVoicePutVoiceConnectorProxyOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId,
[property: CliOption("--default-session-expiry-minutes")] int DefaultSessionExpiryMinutes,
[property: CliOption("--phone-number-pool-countries")] string[] PhoneNumberPoolCountries
) : AwsOptions
{
    [CliOption("--fall-back-phone-number")]
    public string? FallBackPhoneNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}