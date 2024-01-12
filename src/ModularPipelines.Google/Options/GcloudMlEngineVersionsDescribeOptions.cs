using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "versions", "describe")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudMlEngineVersionsDescribeOptionsVersion { get; set; }

    [CommandSwitch("--model")]
    public string Model { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}
