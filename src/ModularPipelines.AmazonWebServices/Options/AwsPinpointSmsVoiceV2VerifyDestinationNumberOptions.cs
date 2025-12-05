using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "verify-destination-number")]
public record AwsPinpointSmsVoiceV2VerifyDestinationNumberOptions(
[property: CliOption("--verified-destination-number-id")] string VerifiedDestinationNumberId,
[property: CliOption("--verification-code")] string VerificationCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}