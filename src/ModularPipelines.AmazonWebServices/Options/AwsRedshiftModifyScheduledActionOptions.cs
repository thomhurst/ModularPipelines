using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "modify-scheduled-action")]
public record AwsRedshiftModifyScheduledActionOptions(
[property: CliOption("--scheduled-action-name")] string ScheduledActionName,
[property: CliOption("--schedule")] string Schedule
) : AwsOptions
{
    [CliOption("--target-action")]
    public string? TargetAction { get; set; }

    [CliOption("--iam-role")]
    public string? IamRole { get; set; }

    [CliOption("--scheduled-action-description")]
    public string? ScheduledActionDescription { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}