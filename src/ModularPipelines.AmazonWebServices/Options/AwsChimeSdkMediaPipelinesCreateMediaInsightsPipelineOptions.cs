using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-media-pipelines", "create-media-insights-pipeline")]
public record AwsChimeSdkMediaPipelinesCreateMediaInsightsPipelineOptions(
[property: CliOption("--media-insights-pipeline-configuration-arn")] string MediaInsightsPipelineConfigurationArn
) : AwsOptions
{
    [CliOption("--kinesis-video-stream-source-runtime-configuration")]
    public string? KinesisVideoStreamSourceRuntimeConfiguration { get; set; }

    [CliOption("--media-insights-runtime-metadata")]
    public IEnumerable<KeyValue>? MediaInsightsRuntimeMetadata { get; set; }

    [CliOption("--kinesis-video-stream-recording-source-runtime-configuration")]
    public string? KinesisVideoStreamRecordingSourceRuntimeConfiguration { get; set; }

    [CliOption("--s3-recording-sink-runtime-configuration")]
    public string? S3RecordingSinkRuntimeConfiguration { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}