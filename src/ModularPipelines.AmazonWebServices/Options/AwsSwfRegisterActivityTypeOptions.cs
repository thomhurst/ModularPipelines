using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "register-activity-type")]
public record AwsSwfRegisterActivityTypeOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--activity-version")] string ActivityVersion
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--default-task-start-to-close-timeout")]
    public string? DefaultTaskStartToCloseTimeout { get; set; }

    [CommandSwitch("--default-task-heartbeat-timeout")]
    public string? DefaultTaskHeartbeatTimeout { get; set; }

    [CommandSwitch("--default-task-list")]
    public string? DefaultTaskList { get; set; }

    [CommandSwitch("--default-task-priority")]
    public string? DefaultTaskPriority { get; set; }

    [CommandSwitch("--default-task-schedule-to-start-timeout")]
    public string? DefaultTaskScheduleToStartTimeout { get; set; }

    [CommandSwitch("--default-task-schedule-to-close-timeout")]
    public string? DefaultTaskScheduleToCloseTimeout { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}