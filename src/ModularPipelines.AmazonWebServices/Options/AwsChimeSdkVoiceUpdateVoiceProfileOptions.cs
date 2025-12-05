using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "update-voice-profile")]
public record AwsChimeSdkVoiceUpdateVoiceProfileOptions(
[property: CliOption("--voice-profile-id")] string VoiceProfileId,
[property: CliOption("--speaker-search-task-id")] string SpeakerSearchTaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}