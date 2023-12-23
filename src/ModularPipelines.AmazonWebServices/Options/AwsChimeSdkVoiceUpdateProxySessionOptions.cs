using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "update-proxy-session")]
public record AwsChimeSdkVoiceUpdateProxySessionOptions(
[property: CommandSwitch("--voice-connector-id")] string VoiceConnectorId,
[property: CommandSwitch("--proxy-session-id")] string ProxySessionId,
[property: CommandSwitch("--capabilities")] string[] Capabilities
) : AwsOptions
{
    [CommandSwitch("--expiry-minutes")]
    public int? ExpiryMinutes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}