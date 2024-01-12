using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitoring", "snoozes", "update")]
public record GcloudMonitoringSnoozesUpdateOptions(
[property: PositionalArgument] string Snooze
) : GcloudOptions
{
    [CommandSwitch("--snooze-from-file")]
    public string? SnoozeFromFile { get; set; }

    [CommandSwitch("--fields")]
    public string[]? Fields { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}