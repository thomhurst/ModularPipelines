using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "findings", "update-marks")]
public record GcloudSccFindingsUpdateMarksOptions(
[property: PositionalArgument] string Finding,
[property: PositionalArgument] string Organization,
[property: PositionalArgument] string Source
) : GcloudOptions
{
    [CommandSwitch("--security-marks")]
    public IEnumerable<KeyValue>? SecurityMarks { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--update-mask")]
    public string? UpdateMask { get; set; }
}