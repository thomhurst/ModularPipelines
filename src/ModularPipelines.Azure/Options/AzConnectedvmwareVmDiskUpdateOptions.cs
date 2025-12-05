using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connectedvmware", "vm", "disk", "update")]
public record AzConnectedvmwareVmDiskUpdateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--controller-key")]
    public string? ControllerKey { get; set; }

    [CliOption("--device-key")]
    public string? DeviceKey { get; set; }

    [CliOption("--disk-mode")]
    public string? DiskMode { get; set; }

    [CliOption("--disk-size")]
    public string? DiskSize { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--unit-number")]
    public string? UnitNumber { get; set; }
}