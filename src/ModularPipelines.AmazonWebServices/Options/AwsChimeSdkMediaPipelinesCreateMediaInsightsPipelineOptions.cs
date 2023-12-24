using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-media-pipelines", "create-media-insights-pipeline")]
public record AwsChimeSdkMediaPipelinesCreateMediaInsightsPipelineOptions(
[property: CommandSwitch("--media-insights-pipeline-configuration-arn")] string MediaInsightsPipelineConfigurationArn
) : AwsOptions
{
    [CommandSwitch("--kinesis-video-stream-source-runtime-configuration")]
    public string? KinesisVideoStreamSourceRuntimeConfiguration { get; set; }

    [CommandSwitch("--media-insights-runtime-metadata")]
    public IEnumerable<KeyValue>? MediaInsightsRuntimeMetadata { get; set; }

    [CommandSwitch("--kinesis-video-stream-recording-source-runtime-configuration")]
    public string? KinesisVideoStreamRecordingSourceRuntimeConfiguration { get; set; }

    [CommandSwitch("--s3-recording-sink-runtime-configuration")]
    public string? S3RecordingSinkRuntimeConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}