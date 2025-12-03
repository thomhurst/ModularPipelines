using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutvision", "describe-model-packaging-job")]
public record AwsLookoutvisionDescribeModelPackagingJobOptions(
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--job-name")] string JobName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}