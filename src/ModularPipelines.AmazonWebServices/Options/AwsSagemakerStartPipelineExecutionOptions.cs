using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "start-pipeline-execution")]
public record AwsSagemakerStartPipelineExecutionOptions(
[property: CliOption("--pipeline-name")] string PipelineName
) : AwsOptions
{
    [CliOption("--pipeline-execution-display-name")]
    public string? PipelineExecutionDisplayName { get; set; }

    [CliOption("--pipeline-parameters")]
    public string[]? PipelineParameters { get; set; }

    [CliOption("--pipeline-execution-description")]
    public string? PipelineExecutionDescription { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--parallelism-configuration")]
    public string? ParallelismConfiguration { get; set; }

    [CliOption("--selective-execution-config")]
    public string? SelectiveExecutionConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}