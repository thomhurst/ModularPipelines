using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lab", "custom-image", "create")]
public record AzLabCustomImageCreateOptions(
[property: CommandSwitch("--lab-name")] string LabName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--os-state")] string OsState,
[property: CommandSwitch("--os-type")] string OsType,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--source-vm-id")] string SourceVmId
) : AzOptions
{
    [CommandSwitch("--author")]
    public string? Author { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }
}

