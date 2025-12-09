using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-model-explainability-job-definition")]
public record AwsSagemakerDescribeModelExplainabilityJobDefinitionOptions(
[property: CliOption("--job-definition-name")] string JobDefinitionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}