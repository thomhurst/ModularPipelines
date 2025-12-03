using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "get-pipeline-execution")]
public record AwsCodepipelineGetPipelineExecutionOptions(
[property: CliOption("--pipeline-name")] string PipelineName,
[property: CliOption("--pipeline-execution-id")] string PipelineExecutionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}