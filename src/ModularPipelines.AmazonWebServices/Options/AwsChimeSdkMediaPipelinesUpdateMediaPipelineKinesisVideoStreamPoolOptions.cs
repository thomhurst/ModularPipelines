using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-media-pipelines", "update-media-pipeline-kinesis-video-stream-pool")]
public record AwsChimeSdkMediaPipelinesUpdateMediaPipelineKinesisVideoStreamPoolOptions(
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--stream-configuration")]
    public string? StreamConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}