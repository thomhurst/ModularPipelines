using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr-serverless", "cancel-job-run")]
public record AwsEmrServerlessCancelJobRunOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--job-run-id")] string JobRunId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}