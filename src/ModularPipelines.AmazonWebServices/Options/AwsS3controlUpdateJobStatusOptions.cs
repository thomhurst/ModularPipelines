using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "update-job-status")]
public record AwsS3controlUpdateJobStatusOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--requested-job-status")] string RequestedJobStatus
) : AwsOptions
{
    [CliOption("--status-update-reason")]
    public string? StatusUpdateReason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}