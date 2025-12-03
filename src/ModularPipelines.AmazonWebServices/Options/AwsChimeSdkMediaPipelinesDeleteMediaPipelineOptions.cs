using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-media-pipelines", "delete-media-pipeline")]
public record AwsChimeSdkMediaPipelinesDeleteMediaPipelineOptions(
[property: CliOption("--media-pipeline-id")] string MediaPipelineId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}