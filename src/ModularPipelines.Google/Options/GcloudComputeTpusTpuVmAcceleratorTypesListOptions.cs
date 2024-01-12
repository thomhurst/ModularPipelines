using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "tpu-vm", "accelerator-types", "list")]
public record GcloudComputeTpusTpuVmAcceleratorTypesListOptions : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}