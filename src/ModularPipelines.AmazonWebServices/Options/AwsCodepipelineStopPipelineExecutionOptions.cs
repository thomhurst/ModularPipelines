using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "stop-pipeline-execution")]
public record AwsCodepipelineStopPipelineExecutionOptions(
[property: CliOption("--pipeline-name")] string PipelineName,
[property: CliOption("--pipeline-execution-id")] string PipelineExecutionId
) : AwsOptions
{
    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}