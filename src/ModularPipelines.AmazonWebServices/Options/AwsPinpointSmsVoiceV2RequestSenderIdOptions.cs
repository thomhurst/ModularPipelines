using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "request-sender-id")]
public record AwsPinpointSmsVoiceV2RequestSenderIdOptions(
[property: CliOption("--sender-id")] string SenderId,
[property: CliOption("--iso-country-code")] string IsoCountryCode
) : AwsOptions
{
    [CliOption("--message-types")]
    public string[]? MessageTypes { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}