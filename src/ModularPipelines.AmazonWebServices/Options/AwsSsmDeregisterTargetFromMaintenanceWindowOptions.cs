using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "deregister-target-from-maintenance-window")]
public record AwsSsmDeregisterTargetFromMaintenanceWindowOptions(
[property: CommandSwitch("--window-id")] string WindowId,
[property: CommandSwitch("--window-target-id")] string WindowTargetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}