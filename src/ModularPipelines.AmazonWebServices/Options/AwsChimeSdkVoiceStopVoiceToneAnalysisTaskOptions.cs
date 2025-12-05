using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "stop-voice-tone-analysis-task")]
public record AwsChimeSdkVoiceStopVoiceToneAnalysisTaskOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId,
[property: CliOption("--voice-tone-analysis-task-id")] string VoiceToneAnalysisTaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}