using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "put-keyword")]
public record AwsPinpointSmsVoiceV2PutKeywordOptions(
[property: CliOption("--origination-identity")] string OriginationIdentity,
[property: CliOption("--keyword")] string Keyword,
[property: CliOption("--keyword-message")] string KeywordMessage
) : AwsOptions
{
    [CliOption("--keyword-action")]
    public string? KeywordAction { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}