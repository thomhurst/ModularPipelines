using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "autoscale", "rule", "copy")]
public record AzMonitorAutoscaleRuleCopyOptions(
[property: CommandSwitch("--autoscale-name")] string AutoscaleName,
[property: CommandSwitch("--dest-schedule")] string DestSchedule,
[property: CommandSwitch("--index")] string Index,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--source-schedule")]
    public string? SourceSchedule { get; set; }
}