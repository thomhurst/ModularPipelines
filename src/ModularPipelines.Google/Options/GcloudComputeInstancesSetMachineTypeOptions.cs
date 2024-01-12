using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "set-machine-type")]
public record GcloudComputeInstancesSetMachineTypeOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--custom-cpu")]
    public string? CustomCpu { get; set; }

    [CommandSwitch("--custom-memory")]
    public string? CustomMemory { get; set; }

    [BooleanCommandSwitch("--custom-extensions")]
    public bool? CustomExtensions { get; set; }

    [CommandSwitch("--custom-vm-type")]
    public string? CustomVmType { get; set; }
}