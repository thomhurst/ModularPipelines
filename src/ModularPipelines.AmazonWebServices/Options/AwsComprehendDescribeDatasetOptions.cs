using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "describe-dataset")]
public record AwsComprehendDescribeDatasetOptions(
[property: CliOption("--dataset-arn")] string DatasetArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}