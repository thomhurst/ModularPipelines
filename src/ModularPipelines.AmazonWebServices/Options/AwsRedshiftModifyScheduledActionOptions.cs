using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "modify-scheduled-action")]
public record AwsRedshiftModifyScheduledActionOptions(
[property: CommandSwitch("--scheduled-action-name")] string ScheduledActionName,
[property: CommandSwitch("--schedule")] string Schedule
) : AwsOptions
{
    [CommandSwitch("--target-action")]
    public string? TargetAction { get; set; }

    [CommandSwitch("--iam-role")]
    public string? IamRole { get; set; }

    [CommandSwitch("--scheduled-action-description")]
    public string? ScheduledActionDescription { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}