using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connectedvmware", "vm", "disk", "add")]
public record AzConnectedvmwareVmDiskAddOptions(
[property: CliOption("--controller-key")] string ControllerKey,
[property: CliOption("--disk-size")] string DiskSize,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--disk-mode")]
    public string? DiskMode { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--unit-number")]
    public string? UnitNumber { get; set; }
}