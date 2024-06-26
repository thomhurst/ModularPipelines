using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "versions", "update")]
public record GcloudMlEngineVersionsUpdateOptions : GcloudOptions
{
    public GcloudMlEngineVersionsUpdateOptions(
        string version,
        string model
    )
    {
        GcloudMlEngineVersionsUpdateOptionsVersion = version;
        Model = model;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudMlEngineVersionsUpdateOptionsVersion { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Model { get; set; }

    [CommandSwitch("--config")]
    public string? Config { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}
