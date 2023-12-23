using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "get-maintenance-window-execution-task-invocation")]
public record AwsSsmGetMaintenanceWindowExecutionTaskInvocationOptions(
[property: CommandSwitch("--window-execution-id")] string WindowExecutionId,
[property: CommandSwitch("--task-id")] string TaskId,
[property: CommandSwitch("--invocation-id")] string InvocationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}