using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "project", "task", "show")]
public record AzDmsProjectTaskShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}