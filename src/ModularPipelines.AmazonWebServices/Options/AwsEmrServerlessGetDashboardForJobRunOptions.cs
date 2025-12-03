using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr-serverless", "get-dashboard-for-job-run")]
public record AwsEmrServerlessGetDashboardForJobRunOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--job-run-id")] string JobRunId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}