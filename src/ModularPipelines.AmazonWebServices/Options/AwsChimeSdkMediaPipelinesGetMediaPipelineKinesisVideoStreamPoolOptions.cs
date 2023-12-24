using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-media-pipelines", "get-media-pipeline-kinesis-video-stream-pool")]
public record AwsChimeSdkMediaPipelinesGetMediaPipelineKinesisVideoStreamPoolOptions(
[property: CommandSwitch("--identifier")] string Identifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}