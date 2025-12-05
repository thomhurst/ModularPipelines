using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "versions", "delete")]
public record GcloudMlEngineVersionsDeleteOptions : GcloudOptions
{
    public GcloudMlEngineVersionsDeleteOptions(
        string version,
        string model
    )
    {
        GcloudMlEngineVersionsDeleteOptionsVersion = version;
        Model = model;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudMlEngineVersionsDeleteOptionsVersion { get; set; }

    [CliOption("--model")]
    public string Model { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}
