using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "delete-model-bias-job-definition")]
public record AwsSagemakerDeleteModelBiasJobDefinitionOptions(
[property: CliOption("--job-definition-name")] string JobDefinitionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}