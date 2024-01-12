using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "tpu-vm", "versions", "list")]
public record GcloudComputeTpusTpuVmVersionsListOptions : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}