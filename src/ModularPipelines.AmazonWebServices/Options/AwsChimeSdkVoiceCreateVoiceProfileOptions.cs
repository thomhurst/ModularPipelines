using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "create-voice-profile")]
public record AwsChimeSdkVoiceCreateVoiceProfileOptions(
[property: CommandSwitch("--speaker-search-task-id")] string SpeakerSearchTaskId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}