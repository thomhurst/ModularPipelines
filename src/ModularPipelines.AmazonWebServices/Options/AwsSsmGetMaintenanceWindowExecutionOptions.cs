using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "get-maintenance-window-execution")]
public record AwsSsmGetMaintenanceWindowExecutionOptions(
[property: CommandSwitch("--window-execution-id")] string WindowExecutionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}