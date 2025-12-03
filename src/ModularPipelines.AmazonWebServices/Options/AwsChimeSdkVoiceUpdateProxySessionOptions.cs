using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "update-proxy-session")]
public record AwsChimeSdkVoiceUpdateProxySessionOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId,
[property: CliOption("--proxy-session-id")] string ProxySessionId,
[property: CliOption("--capabilities")] string[] Capabilities
) : AwsOptions
{
    [CliOption("--expiry-minutes")]
    public int? ExpiryMinutes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}