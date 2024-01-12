using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "assets", "update-marks")]
public record GcloudSccAssetsUpdateMarksOptions(
[property: PositionalArgument] string Asset,
[property: PositionalArgument] string Organization
) : GcloudOptions
{
    [CommandSwitch("--security-marks")]
    public IEnumerable<KeyValue>? SecurityMarks { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--update-mask")]
    public string? UpdateMask { get; set; }
}