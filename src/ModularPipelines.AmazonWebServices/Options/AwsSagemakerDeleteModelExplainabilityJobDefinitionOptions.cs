using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "delete-model-explainability-job-definition")]
public record AwsSagemakerDeleteModelExplainabilityJobDefinitionOptions(
[property: CliOption("--job-definition-name")] string JobDefinitionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}