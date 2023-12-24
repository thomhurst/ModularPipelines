using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "delete-voice-profile")]
public record AwsChimeSdkVoiceDeleteVoiceProfileOptions(
[property: CommandSwitch("--voice-profile-id")] string VoiceProfileId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}