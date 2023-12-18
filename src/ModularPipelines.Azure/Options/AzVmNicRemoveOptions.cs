using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "nic", "remove")]
public record AzVmNicRemoveOptions(
[property: CommandSwitch("--nics")] string Nics,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--primary-nic")]
    public string? PrimaryNic { get; set; }
}