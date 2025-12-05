using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-media-pipelines", "start-voice-tone-analysis-task")]
public record AwsChimeSdkMediaPipelinesStartVoiceToneAnalysisTaskOptions(
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--language-code")] string LanguageCode
) : AwsOptions
{
    [CliOption("--kinesis-video-stream-source-task-configuration")]
    public string? KinesisVideoStreamSourceTaskConfiguration { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}