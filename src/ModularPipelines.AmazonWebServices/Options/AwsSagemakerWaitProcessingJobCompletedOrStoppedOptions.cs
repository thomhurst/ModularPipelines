using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "wait", "processing-job-completed-or-stopped")]
public record AwsSagemakerWaitProcessingJobCompletedOrStoppedOptions(
[property: CliOption("--processing-job-name")] string ProcessingJobName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}