using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "list-proxy-sessions")]
public record AwsChimeSdkVoiceListProxySessionsOptions(
[property: CommandSwitch("--voice-connector-id")] string VoiceConnectorId
) : AwsOptions
{
    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}