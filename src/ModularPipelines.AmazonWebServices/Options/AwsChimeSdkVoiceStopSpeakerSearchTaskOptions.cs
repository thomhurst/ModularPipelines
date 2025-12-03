using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "stop-speaker-search-task")]
public record AwsChimeSdkVoiceStopSpeakerSearchTaskOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId,
[property: CliOption("--speaker-search-task-id")] string SpeakerSearchTaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}