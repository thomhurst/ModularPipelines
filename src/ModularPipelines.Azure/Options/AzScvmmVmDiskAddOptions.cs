using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scvmm", "vm", "disk", "add")]
public record AzScvmmVmDiskAddOptions(
[property: CliOption("--bus")] string Bus,
[property: CliOption("--disk-size")] string DiskSize,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--bus-type")]
    public string? BusType { get; set; }

    [CliOption("--lun")]
    public string? Lun { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--qos-id")]
    public string? QosId { get; set; }

    [CliOption("--qos-name")]
    public string? QosName { get; set; }

    [CliOption("--vhd-type")]
    public string? VhdType { get; set; }
}