using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "request-phone-number")]
public record AwsPinpointSmsVoiceV2RequestPhoneNumberOptions(
[property: CommandSwitch("--iso-country-code")] string IsoCountryCode,
[property: CommandSwitch("--message-type")] string MessageType,
[property: CommandSwitch("--number-capabilities")] string[] NumberCapabilities,
[property: CommandSwitch("--number-type")] string NumberType
) : AwsOptions
{
    [CommandSwitch("--opt-out-list-name")]
    public string? OptOutListName { get; set; }

    [CommandSwitch("--pool-id")]
    public string? PoolId { get; set; }

    [CommandSwitch("--registration-id")]
    public string? RegistrationId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}