using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-media-pipelines", "create-media-pipeline-kinesis-video-stream-pool")]
public record AwsChimeSdkMediaPipelinesCreateMediaPipelineKinesisVideoStreamPoolOptions(
[property: CliOption("--stream-configuration")] string StreamConfiguration,
[property: CliOption("--pool-name")] string PoolName
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}