using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss", "run-command", "list")]
public record AzVmssRunCommandListOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vmss-name")] string VmssName
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}

