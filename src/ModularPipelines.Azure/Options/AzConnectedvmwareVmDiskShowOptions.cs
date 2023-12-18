using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedvmware", "vm", "disk", "show")]
public record AzConnectedvmwareVmDiskShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--controller-key")]
    public string? ControllerKey { get; set; }

    [CommandSwitch("--device-key")]
    public string? DeviceKey { get; set; }

    [CommandSwitch("--disk-mode")]
    public string? DiskMode { get; set; }

    [CommandSwitch("--disk-size")]
    public string? DiskSize { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--unit-number")]
    public string? UnitNumber { get; set; }
}