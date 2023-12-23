using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "register-task-with-maintenance-window")]
public record AwsSsmRegisterTaskWithMaintenanceWindowOptions(
[property: CommandSwitch("--window-id")] string WindowId,
[property: CommandSwitch("--task-arn")] string TaskArn,
[property: CommandSwitch("--task-type")] string TaskType
) : AwsOptions
{
    [CommandSwitch("--targets")]
    public string[]? Targets { get; set; }

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

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--cutoff-behavior")]
    public string? CutoffBehavior { get; set; }

    [CommandSwitch("--alarm-configuration")]
    public string? AlarmConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}