using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("scvmm", "vm", "disk", "update")]
public record AzScvmmVmDiskUpdateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--bus")]
    public string? Bus { get; set; }

    [CliOption("--bus-type")]
    public string? BusType { get; set; }

    [CliOption("--disk-id")]
    public string? DiskId { get; set; }

    [CliOption("--disk-size")]
    public string? DiskSize { get; set; }

    [CliOption("--lun")]
    public string? Lun { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--qos-id")]
    public string? QosId { get; set; }

    [CliOption("--qos-name")]
    public string? QosName { get; set; }

    [CliOption("--vhd-type")]
    public string? VhdType { get; set; }
}