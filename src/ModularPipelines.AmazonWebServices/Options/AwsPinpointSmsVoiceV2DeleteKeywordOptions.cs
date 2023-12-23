using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "delete-keyword")]
public record AwsPinpointSmsVoiceV2DeleteKeywordOptions(
[property: CommandSwitch("--origination-identity")] string OriginationIdentity,
[property: CommandSwitch("--keyword")] string Keyword
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}