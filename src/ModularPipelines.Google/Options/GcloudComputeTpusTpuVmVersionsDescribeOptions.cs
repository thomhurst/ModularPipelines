using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "tpu-vm", "versions", "describe")]
public record GcloudComputeTpusTpuVmVersionsDescribeOptions : GcloudOptions
{
    public GcloudComputeTpusTpuVmVersionsDescribeOptions(
        string version,
        string zone
    )
    {
        GcloudComputeTpusTpuVmVersionsDescribeOptionsVersion = version;
        Zone = zone;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudComputeTpusTpuVmVersionsDescribeOptionsVersion { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Zone { get; set; }
}
