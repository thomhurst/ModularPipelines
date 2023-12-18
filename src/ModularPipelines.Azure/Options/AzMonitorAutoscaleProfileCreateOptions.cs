using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "autoscale", "profile", "create")]
public record AzMonitorAutoscaleProfileCreateOptions(
[property: CommandSwitch("--autoscale-name")] string AutoscaleName,
[property: CommandSwitch("--count")] int Count,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--timezone")] string Timezone
) : AzOptions
{
    [CommandSwitch("--copy-rules")]
    public string? CopyRules { get; set; }

    [CommandSwitch("--end")]
    public string? End { get; set; }

    [CommandSwitch("--max-count")]
    public int? MaxCount { get; set; }

    [CommandSwitch("--min-count")]
    public int? MinCount { get; set; }

    [CommandSwitch("--recurrence")]
    public string? Recurrence { get; set; }

    [CommandSwitch("--start")]
    public string? Start { get; set; }
}