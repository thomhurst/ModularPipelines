using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "create-scheduled-action")]
public record AwsRedshiftCreateScheduledActionOptions(
[property: CommandSwitch("--scheduled-action-name")] string ScheduledActionName,
[property: CommandSwitch("--target-action")] string TargetAction,
[property: CommandSwitch("--schedule")] string Schedule,
[property: CommandSwitch("--iam-role")] string IamRole
) : AwsOptions
{
    [CommandSwitch("--scheduled-action-description")]
    public string? ScheduledActionDescription { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}