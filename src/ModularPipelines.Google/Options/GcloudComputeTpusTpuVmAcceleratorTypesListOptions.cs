using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "tpu-vm", "accelerator-types", "list")]
public record GcloudComputeTpusTpuVmAcceleratorTypesListOptions : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}