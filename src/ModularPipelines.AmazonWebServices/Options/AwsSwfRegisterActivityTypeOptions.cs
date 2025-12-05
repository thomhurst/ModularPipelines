using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "register-activity-type")]
public record AwsSwfRegisterActivityTypeOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--name")] string Name,
[property: CliOption("--activity-version")] string ActivityVersion
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--default-task-start-to-close-timeout")]
    public string? DefaultTaskStartToCloseTimeout { get; set; }

    [CliOption("--default-task-heartbeat-timeout")]
    public string? DefaultTaskHeartbeatTimeout { get; set; }

    [CliOption("--default-task-list")]
    public string? DefaultTaskList { get; set; }

    [CliOption("--default-task-priority")]
    public string? DefaultTaskPriority { get; set; }

    [CliOption("--default-task-schedule-to-start-timeout")]
    public string? DefaultTaskScheduleToStartTimeout { get; set; }

    [CliOption("--default-task-schedule-to-close-timeout")]
    public string? DefaultTaskScheduleToCloseTimeout { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}