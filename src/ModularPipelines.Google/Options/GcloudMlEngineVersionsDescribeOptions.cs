using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "versions", "describe")]
public record GcloudMlEngineVersionsDescribeOptions : GcloudOptions
{
    public GcloudMlEngineVersionsDescribeOptions(
        string version,
        string model
    )
    {
        GcloudMlEngineVersionsDescribeOptionsVersion = version;
        Model = model;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudMlEngineVersionsDescribeOptionsVersion { get; set; }

    [CliOption("--model")]
    public string Model { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}
