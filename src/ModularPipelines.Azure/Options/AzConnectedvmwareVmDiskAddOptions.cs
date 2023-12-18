using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedvmware", "vm", "disk", "add")]
public record AzConnectedvmwareVmDiskAddOptions(
[property: CommandSwitch("--controller-key")] string ControllerKey,
[property: CommandSwitch("--disk-size")] string DiskSize,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--disk-mode")]
    public string? DiskMode { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--unit-number")]
    public string? UnitNumber { get; set; }
}