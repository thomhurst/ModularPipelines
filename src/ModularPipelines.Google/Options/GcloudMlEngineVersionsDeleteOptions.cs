using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "versions", "delete")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudMlEngineVersionsDeleteOptionsVersion { get; set; }

    [CommandSwitch("--model")]
    public string Model { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}
