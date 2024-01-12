using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "tpu-vm", "service-identity", "create")]
public record GcloudComputeTpusTpuVmServiceIdentityCreateOptions : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}