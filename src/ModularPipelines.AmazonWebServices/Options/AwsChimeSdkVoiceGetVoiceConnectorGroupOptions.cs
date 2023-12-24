using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "get-voice-connector-group")]
public record AwsChimeSdkVoiceGetVoiceConnectorGroupOptions(
[property: CommandSwitch("--voice-connector-group-id")] string VoiceConnectorGroupId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}