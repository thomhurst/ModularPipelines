using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "stop-edge-packaging-job")]
public record AwsSagemakerStopEdgePackagingJobOptions(
[property: CliOption("--edge-packaging-job-name")] string EdgePackagingJobName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}