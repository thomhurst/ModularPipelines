using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "retry-stage-execution")]
public record AwsCodepipelineRetryStageExecutionOptions(
[property: CliOption("--pipeline-name")] string PipelineName,
[property: CliOption("--stage-name")] string StageName,
[property: CliOption("--pipeline-execution-id")] string PipelineExecutionId,
[property: CliOption("--retry-mode")] string RetryMode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}