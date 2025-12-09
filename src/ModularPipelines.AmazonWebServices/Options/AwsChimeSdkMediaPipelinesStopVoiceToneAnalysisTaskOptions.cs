using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-media-pipelines", "stop-voice-tone-analysis-task")]
public record AwsChimeSdkMediaPipelinesStopVoiceToneAnalysisTaskOptions(
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--voice-tone-analysis-task-id")] string VoiceToneAnalysisTaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}