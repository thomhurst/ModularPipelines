using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "tpu-vm", "versions", "describe")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudComputeTpusTpuVmVersionsDescribeOptionsVersion { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Zone { get; set; }
}
