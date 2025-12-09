using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "describe-dataset-export-job")]
public record AwsPersonalizeDescribeDatasetExportJobOptions(
[property: CliOption("--dataset-export-job-arn")] string DatasetExportJobArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}