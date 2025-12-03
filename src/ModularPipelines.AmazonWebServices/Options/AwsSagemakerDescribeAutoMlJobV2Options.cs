using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-auto-ml-job-v2")]
public record AwsSagemakerDescribeAutoMlJobV2Options(
[property: CliOption("--auto-ml-job-name")] string AutoMlJobName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}