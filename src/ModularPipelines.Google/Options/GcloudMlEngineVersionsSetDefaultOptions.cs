using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "versions", "set-default")]
public record GcloudMlEngineVersionsSetDefaultOptions : GcloudOptions
{
    public GcloudMlEngineVersionsSetDefaultOptions(
        string version,
        string model
    )
    {
        GcloudMlEngineVersionsSetDefaultOptionsVersion = version;
        Model = model;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudMlEngineVersionsSetDefaultOptionsVersion { get; set; }

    [CliOption("--model")]
    public string Model { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}
