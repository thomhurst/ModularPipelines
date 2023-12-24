using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-media-pipelines", "stop-voice-tone-analysis-task")]
public record AwsChimeSdkMediaPipelinesStopVoiceToneAnalysisTaskOptions(
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--voice-tone-analysis-task-id")] string VoiceToneAnalysisTaskId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}