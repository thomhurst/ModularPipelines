using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-media-pipelines", "delete-media-pipeline")]
public record AwsChimeSdkMediaPipelinesDeleteMediaPipelineOptions(
[property: CommandSwitch("--media-pipeline-id")] string MediaPipelineId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}