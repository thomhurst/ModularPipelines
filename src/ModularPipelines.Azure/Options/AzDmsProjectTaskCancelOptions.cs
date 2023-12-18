using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "project", "task", "cancel")]
public record AzDmsProjectTaskCancelOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
    [CommandSwitch("--object-name")]
    public string? ObjectName { get; set; }
}