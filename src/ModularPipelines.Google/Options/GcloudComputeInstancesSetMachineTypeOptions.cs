using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "set-machine-type")]
public record GcloudComputeInstancesSetMachineTypeOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--custom-cpu")]
    public string? CustomCpu { get; set; }

    [CliOption("--custom-memory")]
    public string? CustomMemory { get; set; }

    [CliFlag("--custom-extensions")]
    public bool? CustomExtensions { get; set; }

    [CliOption("--custom-vm-type")]
    public string? CustomVmType { get; set; }
}