using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-model-card-export-job")]
public record AwsSagemakerDescribeModelCardExportJobOptions(
[property: CliOption("--model-card-export-job-arn")] string ModelCardExportJobArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}