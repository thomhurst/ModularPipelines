using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "list-proxy-sessions")]
public record AwsChimeSdkVoiceListProxySessionsOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId
) : AwsOptions
{
    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}