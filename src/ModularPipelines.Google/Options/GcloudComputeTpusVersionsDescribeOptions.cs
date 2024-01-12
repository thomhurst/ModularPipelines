using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "versions", "describe")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudComputeTpusVersionsDescribeOptionsVersion { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Zone { get; set; }
}
