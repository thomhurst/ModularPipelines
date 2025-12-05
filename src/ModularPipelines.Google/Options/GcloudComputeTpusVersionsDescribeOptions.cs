using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "versions", "describe")]
public record GcloudComputeTpusVersionsDescribeOptions : GcloudOptions
{
    public GcloudComputeTpusVersionsDescribeOptions(
        string version,
        string zone
    )
    {
        GcloudComputeTpusVersionsDescribeOptionsVersion = version;
        Zone = zone;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudComputeTpusVersionsDescribeOptionsVersion { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Zone { get; set; }
}
