using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "versions", "set-default")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudMlEngineVersionsSetDefaultOptionsVersion { get; set; }

    [CommandSwitch("--model")]
    public string Model { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}
