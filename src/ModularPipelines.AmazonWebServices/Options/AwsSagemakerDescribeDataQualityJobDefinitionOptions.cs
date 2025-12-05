using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-data-quality-job-definition")]
public record AwsSagemakerDescribeDataQualityJobDefinitionOptions(
[property: CliOption("--job-definition-name")] string JobDefinitionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}