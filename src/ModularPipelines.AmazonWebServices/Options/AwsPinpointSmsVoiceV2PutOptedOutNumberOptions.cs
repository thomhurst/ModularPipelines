using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "put-opted-out-number")]
public record AwsPinpointSmsVoiceV2PutOptedOutNumberOptions(
[property: CommandSwitch("--opt-out-list-name")] string OptOutListName,
[property: CommandSwitch("--opted-out-number")] string OptedOutNumber
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}