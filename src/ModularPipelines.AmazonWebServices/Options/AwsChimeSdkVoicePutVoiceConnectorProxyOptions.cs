using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "put-voice-connector-proxy")]
public record AwsChimeSdkVoicePutVoiceConnectorProxyOptions(
[property: CommandSwitch("--voice-connector-id")] string VoiceConnectorId,
[property: CommandSwitch("--default-session-expiry-minutes")] int DefaultSessionExpiryMinutes,
[property: CommandSwitch("--phone-number-pool-countries")] string[] PhoneNumberPoolCountries
) : AwsOptions
{
    [CommandSwitch("--fall-back-phone-number")]
    public string? FallBackPhoneNumber { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}