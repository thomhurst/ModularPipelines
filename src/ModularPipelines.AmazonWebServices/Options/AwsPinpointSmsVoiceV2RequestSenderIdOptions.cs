using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "request-sender-id")]
public record AwsPinpointSmsVoiceV2RequestSenderIdOptions(
[property: CommandSwitch("--sender-id")] string SenderId,
[property: CommandSwitch("--iso-country-code")] string IsoCountryCode
) : AwsOptions
{
    [CommandSwitch("--message-types")]
    public string[]? MessageTypes { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}