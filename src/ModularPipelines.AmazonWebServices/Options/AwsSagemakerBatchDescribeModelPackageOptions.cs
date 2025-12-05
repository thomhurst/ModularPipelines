using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "batch-describe-model-package")]
public record AwsSagemakerBatchDescribeModelPackageOptions(
[property: CliOption("--model-package-arn-list")] string[] ModelPackageArnList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}