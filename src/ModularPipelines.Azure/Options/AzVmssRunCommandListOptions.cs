using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

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