using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "assets", "update-marks")]
public record GcloudSccAssetsUpdateMarksOptions(
[property: CliArgument] string Asset,
[property: CliArgument] string Organization
) : GcloudOptions
{
    [CliOption("--security-marks")]
    public IEnumerable<KeyValue>? SecurityMarks { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--update-mask")]
    public string? UpdateMask { get; set; }
}