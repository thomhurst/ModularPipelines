using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm", "vm", "disk", "show")]
public record AzScvmmVmDiskShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--bus")]
    public string? Bus { get; set; }

    [CommandSwitch("--bus-type")]
    public string? BusType { get; set; }

    [CommandSwitch("--disk-id")]
    public string? DiskId { get; set; }

    [CommandSwitch("--disk-size")]
    public string? DiskSize { get; set; }

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