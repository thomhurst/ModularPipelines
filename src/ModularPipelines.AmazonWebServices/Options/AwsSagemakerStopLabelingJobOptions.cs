using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "stop-labeling-job")]
public record AwsSagemakerStopLabelingJobOptions(
[property: CliOption("--labeling-job-name")] string LabelingJobName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}