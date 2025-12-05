using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-pipeline-execution")]
public record AwsSagemakerUpdatePipelineExecutionOptions(
[property: CliOption("--pipeline-execution-arn")] string PipelineExecutionArn
) : AwsOptions
{
    [CliOption("--pipeline-execution-description")]
    public string? PipelineExecutionDescription { get; set; }

    [CliOption("--pipeline-execution-display-name")]
    public string? PipelineExecutionDisplayName { get; set; }

    [CliOption("--parallelism-configuration")]
    public string? ParallelismConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}