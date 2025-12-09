using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "request-phone-number")]
public record AwsPinpointSmsVoiceV2RequestPhoneNumberOptions(
[property: CliOption("--iso-country-code")] string IsoCountryCode,
[property: CliOption("--message-type")] string MessageType,
[property: CliOption("--number-capabilities")] string[] NumberCapabilities,
[property: CliOption("--number-type")] string NumberType
) : AwsOptions
{
    [CliOption("--opt-out-list-name")]
    public string? OptOutListName { get; set; }

    [CliOption("--pool-id")]
    public string? PoolId { get; set; }

    [CliOption("--registration-id")]
    public string? RegistrationId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}