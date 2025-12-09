using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-pipeline-definition-for-execution")]
public record AwsSagemakerDescribePipelineDefinitionForExecutionOptions(
[property: CliOption("--pipeline-execution-arn")] string PipelineExecutionArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}