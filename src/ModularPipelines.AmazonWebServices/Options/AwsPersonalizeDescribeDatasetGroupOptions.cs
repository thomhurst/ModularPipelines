using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "describe-dataset-group")]
public record AwsPersonalizeDescribeDatasetGroupOptions(
[property: CliOption("--dataset-group-arn")] string DatasetGroupArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}