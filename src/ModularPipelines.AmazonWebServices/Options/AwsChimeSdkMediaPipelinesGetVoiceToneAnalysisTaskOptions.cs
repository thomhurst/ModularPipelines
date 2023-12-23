using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-media-pipelines", "get-voice-tone-analysis-task")]
public record AwsChimeSdkMediaPipelinesGetVoiceToneAnalysisTaskOptions(
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--voice-tone-analysis-task-id")] string VoiceToneAnalysisTaskId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}