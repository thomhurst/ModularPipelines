using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "tpu-vm", "versions", "list")]
public record GcloudComputeTpusTpuVmVersionsListOptions : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}