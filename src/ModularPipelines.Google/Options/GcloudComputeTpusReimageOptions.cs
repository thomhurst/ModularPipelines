using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "reimage")]
public record GcloudComputeTpusReimageOptions : GcloudOptions
{
    public GcloudComputeTpusReimageOptions(
        string tpu,
        string zone,
        string version
    )
    {
        Tpu = tpu;
        Zone = zone;
        Version = version;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Tpu { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Zone { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}
