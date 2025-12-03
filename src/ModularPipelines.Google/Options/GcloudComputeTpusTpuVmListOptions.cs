using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "tpu-vm", "list")]
public record GcloudComputeTpusTpuVmListOptions : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}