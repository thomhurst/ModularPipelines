using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack-hci-vm", "disk", "detach")]
public record AzStackHciVmDiskDetachOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}