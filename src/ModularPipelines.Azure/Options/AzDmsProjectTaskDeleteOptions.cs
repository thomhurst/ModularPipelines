using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "project", "task", "delete")]
public record AzDmsProjectTaskDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
    [CommandSwitch("--delete-running-tasks")]
    public string? DeleteRunningTasks { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}