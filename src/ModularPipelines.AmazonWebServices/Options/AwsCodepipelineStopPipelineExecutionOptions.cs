using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "stop-pipeline-execution")]
public record AwsCodepipelineStopPipelineExecutionOptions(
[property: CommandSwitch("--pipeline-name")] string PipelineName,
[property: CommandSwitch("--pipeline-execution-id")] string PipelineExecutionId
) : AwsOptions
{
    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}