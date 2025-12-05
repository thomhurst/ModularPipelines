using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "create-scheduled-action")]
public record AwsRedshiftCreateScheduledActionOptions(
[property: CliOption("--scheduled-action-name")] string ScheduledActionName,
[property: CliOption("--target-action")] string TargetAction,
[property: CliOption("--schedule")] string Schedule,
[property: CliOption("--iam-role")] string IamRole
) : AwsOptions
{
    [CliOption("--scheduled-action-description")]
    public string? ScheduledActionDescription { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}