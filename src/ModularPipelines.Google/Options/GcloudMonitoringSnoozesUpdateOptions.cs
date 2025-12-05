using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitoring", "snoozes", "update")]
public record GcloudMonitoringSnoozesUpdateOptions(
[property: CliArgument] string Snooze
) : GcloudOptions
{
    [CliOption("--snooze-from-file")]
    public string? SnoozeFromFile { get; set; }

    [CliOption("--fields")]
    public string[]? Fields { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}