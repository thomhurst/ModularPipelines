using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "verify-destination-number")]
public record AwsPinpointSmsVoiceV2VerifyDestinationNumberOptions(
[property: CommandSwitch("--verified-destination-number-id")] string VerifiedDestinationNumberId,
[property: CommandSwitch("--verification-code")] string VerificationCode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}