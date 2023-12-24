using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "update-maintenance-window-task")]
public record AwsSsmUpdateMaintenanceWindowTaskOptions(
[property: CommandSwitch("--window-id")] string WindowId,
[property: CommandSwitch("--window-task-id")] string WindowTaskId
) : AwsOptions
{
    [CommandSwitch("--targets")]
    public string[]? Targets { get; set; }

    [CommandSwitch("--task-arn")]
    public string? TaskArn { get; set; }

    [CommandSwitch("--service-role-arn")]
    public string? ServiceRoleArn { get; set; }

    [CommandSwitch("--task-parameters")]
    public IEnumerable<KeyValue>? TaskParameters { get; set; }

    [CommandSwitch("--task-invocation-parameters")]
    public string? TaskInvocationParameters { get; set; }

    [CommandSwitch("--priority")]
    public int? Priority { get; set; }

    [CommandSwitch("--max-concurrency")]
    public string? MaxConcurrency { get; set; }

    [CommandSwitch("--max-errors")]
    public string? MaxErrors { get; set; }

    [CommandSwitch("--logging-info")]
    public string? LoggingInfo { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--cutoff-behavior")]
    public string? CutoffBehavior { get; set; }

    [CommandSwitch("--alarm-configuration")]
    public string? AlarmConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}