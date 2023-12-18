using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm", "vm", "disk", "add")]
public record AzScvmmVmDiskAddOptions(
[property: CommandSwitch("--bus")] string Bus,
[property: CommandSwitch("--disk-size")] string DiskSize,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--bus-type")]
    public string? BusType { get; set; }

    [CommandSwitch("--lun")]
    public string? Lun { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--qos-id")]
    public string? QosId { get; set; }

    [CommandSwitch("--qos-name")]
    public string? QosName { get; set; }

    [CommandSwitch("--vhd-type")]
    public string? VhdType { get; set; }
}