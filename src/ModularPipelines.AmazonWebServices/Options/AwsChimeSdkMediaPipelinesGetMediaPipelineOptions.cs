using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-media-pipelines", "get-media-pipeline")]
public record AwsChimeSdkMediaPipelinesGetMediaPipelineOptions(
[property: CommandSwitch("--media-pipeline-id")] string MediaPipelineId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}