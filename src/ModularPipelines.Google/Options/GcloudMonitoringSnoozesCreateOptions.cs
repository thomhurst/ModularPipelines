using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitoring", "snoozes", "create")]
public record GcloudMonitoringSnoozesCreateOptions : GcloudOptions
{
    [CommandSwitch("--snooze-from-file")]
    public string? SnoozeFromFile { get; set; }

    [CommandSwitch("--criteria-policies")]
    public string[]? CriteriaPolicies { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}