using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "update-job-status")]
public record AwsS3controlUpdateJobStatusOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--requested-job-status")] string RequestedJobStatus
) : AwsOptions
{
    [CommandSwitch("--status-update-reason")]
    public string? StatusUpdateReason { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}