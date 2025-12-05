using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-hyper-parameter-tuning-job")]
public record AwsSagemakerDescribeHyperParameterTuningJobOptions(
[property: CliOption("--hyper-parameter-tuning-job-name")] string HyperParameterTuningJobName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}