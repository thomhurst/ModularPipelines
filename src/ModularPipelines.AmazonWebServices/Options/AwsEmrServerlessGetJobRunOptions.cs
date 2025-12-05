using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr-serverless", "get-job-run")]
public record AwsEmrServerlessGetJobRunOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--job-run-id")] string JobRunId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}