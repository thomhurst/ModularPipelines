using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "create-pool")]
public record AwsPinpointSmsVoiceV2CreatePoolOptions(
[property: CommandSwitch("--origination-identity")] string OriginationIdentity,
[property: CommandSwitch("--iso-country-code")] string IsoCountryCode,
[property: CommandSwitch("--message-type")] string MessageType
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}