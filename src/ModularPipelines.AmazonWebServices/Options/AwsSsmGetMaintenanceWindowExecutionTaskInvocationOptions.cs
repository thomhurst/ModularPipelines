using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "get-maintenance-window-execution-task-invocation")]
public record AwsSsmGetMaintenanceWindowExecutionTaskInvocationOptions(
[property: CliOption("--window-execution-id")] string WindowExecutionId,
[property: CliOption("--task-id")] string TaskId,
[property: CliOption("--invocation-id")] string InvocationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}