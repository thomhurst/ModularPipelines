using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "update-voice-connector-group")]
public record AwsChimeSdkVoiceUpdateVoiceConnectorGroupOptions(
[property: CommandSwitch("--voice-connector-group-id")] string VoiceConnectorGroupId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--voice-connector-items")] string[] VoiceConnectorItems
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}