using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "tpu-vm", "service-identity", "create")]
public record GcloudComputeTpusTpuVmServiceIdentityCreateOptions : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}