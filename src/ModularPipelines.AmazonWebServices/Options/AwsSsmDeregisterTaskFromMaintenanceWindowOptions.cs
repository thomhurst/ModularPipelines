using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "deregister-task-from-maintenance-window")]
public record AwsSsmDeregisterTaskFromMaintenanceWindowOptions(
[property: CommandSwitch("--window-id")] string WindowId,
[property: CommandSwitch("--window-task-id")] string WindowTaskId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}