using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "update-sender-id")]
public record AwsPinpointSmsVoiceV2UpdateSenderIdOptions(
[property: CommandSwitch("--sender-id")] string SenderId,
[property: CommandSwitch("--iso-country-code")] string IsoCountryCode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}