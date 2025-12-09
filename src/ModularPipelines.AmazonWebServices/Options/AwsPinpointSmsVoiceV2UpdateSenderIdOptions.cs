using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "update-sender-id")]
public record AwsPinpointSmsVoiceV2UpdateSenderIdOptions(
[property: CliOption("--sender-id")] string SenderId,
[property: CliOption("--iso-country-code")] string IsoCountryCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}