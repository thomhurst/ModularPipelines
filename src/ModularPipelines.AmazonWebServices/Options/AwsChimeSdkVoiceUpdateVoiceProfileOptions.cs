using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "update-voice-profile")]
public record AwsChimeSdkVoiceUpdateVoiceProfileOptions(
[property: CommandSwitch("--voice-profile-id")] string VoiceProfileId,
[property: CommandSwitch("--speaker-search-task-id")] string SpeakerSearchTaskId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}