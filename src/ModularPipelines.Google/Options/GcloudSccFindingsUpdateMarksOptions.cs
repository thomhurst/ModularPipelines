using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "findings", "update-marks")]
public record GcloudSccFindingsUpdateMarksOptions(
[property: CliArgument] string Finding,
[property: CliArgument] string Organization,
[property: CliArgument] string Source
) : GcloudOptions
{
    [CliOption("--security-marks")]
    public IEnumerable<KeyValue>? SecurityMarks { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--update-mask")]
    public string? UpdateMask { get; set; }
}