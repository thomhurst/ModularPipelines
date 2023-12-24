using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "put-keyword")]
public record AwsPinpointSmsVoiceV2PutKeywordOptions(
[property: CommandSwitch("--origination-identity")] string OriginationIdentity,
[property: CommandSwitch("--keyword")] string Keyword,
[property: CommandSwitch("--keyword-message")] string KeywordMessage
) : AwsOptions
{
    [CommandSwitch("--keyword-action")]
    public string? KeywordAction { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}