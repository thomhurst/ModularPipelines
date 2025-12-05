using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "retry-pipeline-execution")]
public record AwsSagemakerRetryPipelineExecutionOptions(
[property: CliOption("--pipeline-execution-arn")] string PipelineExecutionArn
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--parallelism-configuration")]
    public string? ParallelismConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}