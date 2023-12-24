using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "retry-stage-execution")]
public record AwsCodepipelineRetryStageExecutionOptions(
[property: CommandSwitch("--pipeline-name")] string PipelineName,
[property: CommandSwitch("--stage-name")] string StageName,
[property: CommandSwitch("--pipeline-execution-id")] string PipelineExecutionId,
[property: CommandSwitch("--retry-mode")] string RetryMode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}