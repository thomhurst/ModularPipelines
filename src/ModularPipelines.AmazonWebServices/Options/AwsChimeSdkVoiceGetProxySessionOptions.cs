using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "get-proxy-session")]
public record AwsChimeSdkVoiceGetProxySessionOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId,
[property: CliOption("--proxy-session-id")] string ProxySessionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}