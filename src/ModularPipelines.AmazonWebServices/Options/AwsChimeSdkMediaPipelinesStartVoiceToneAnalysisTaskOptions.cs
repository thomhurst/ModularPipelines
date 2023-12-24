using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-media-pipelines", "start-voice-tone-analysis-task")]
public record AwsChimeSdkMediaPipelinesStartVoiceToneAnalysisTaskOptions(
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--language-code")] string LanguageCode
) : AwsOptions
{
    [CommandSwitch("--kinesis-video-stream-source-task-configuration")]
    public string? KinesisVideoStreamSourceTaskConfiguration { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}