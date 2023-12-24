using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "get-pipeline-execution")]
public record AwsCodepipelineGetPipelineExecutionOptions(
[property: CommandSwitch("--pipeline-name")] string PipelineName,
[property: CommandSwitch("--pipeline-execution-id")] string PipelineExecutionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}