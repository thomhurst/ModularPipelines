using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "reimage")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Tpu { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Zone { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }
}
