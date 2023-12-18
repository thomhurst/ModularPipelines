using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "waf-log-analytic", "metric", "list")]
public record AzAfdWafLogAnalyticMetricListOptions(
[property: CommandSwitch("--date-time-begin")] string DateTimeBegin,
[property: CommandSwitch("--date-time-end")] string DateTimeEnd,
[property: CommandSwitch("--granularity")] string Granularity,
[property: CommandSwitch("--metrics")] string Metrics,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--actions")]
    public string? Actions { get; set; }

    [CommandSwitch("--group-by")]
    public string? GroupBy { get; set; }

    [CommandSwitch("--rule-types")]
    public string? RuleTypes { get; set; }
}