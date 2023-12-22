using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "data-collection", "rule", "performance-counter", "update")]
public record AzMonitorDataCollectionRulePerformanceCounterUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--counter-specifiers")]
    public int? CounterSpecifiers { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-name")]
    public string? RuleName { get; set; }

    [CommandSwitch("--sampling-frequency")]
    public string? SamplingFrequency { get; set; }

    [CommandSwitch("--streams")]
    public string? Streams { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}